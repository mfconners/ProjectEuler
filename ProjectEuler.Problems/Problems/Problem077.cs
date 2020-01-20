using ProjectEuler.MathExtensions;
using System.Collections.Generic;

namespace ProjectEuler.Problems
{
	class Problem077 : Problem
	{
		public override string CorrectAnswer { get { return "71"; } }

		protected override string CalculateSolution()
		{
			int max_sum = 0;
			int max_count = 1;
			Dictionary<(int p, int sum), int> sum_count = new Dictionary<(int p, int sum), int>();

			while (max_count <= 5000)
			{
				++max_sum;
				max_count = 0;
				for (int p = 0, prime; (prime = Primes.GetPrime(p)) <= max_sum; ++p)
				{
					int prev_sum = max_sum - prime;
					int count = 0;

					if (prev_sum > 1)
					{
						for (int prev_p = 0, prev_prime; prev_p <= p && (prev_prime = Primes.GetPrime(prev_p)) <= prev_sum; ++prev_p)
						{
							if (sum_count.ContainsKey((prev_p, prev_sum)))
							{
								count += sum_count[(prev_p, prev_sum)];
							}
						}
					}
					else if (prev_sum == 0)
					{
						count = 1;
					}

					if (count > 0)
					{
						sum_count.Add((p, max_sum), count);
						max_count += count;
					}
				}
			}

			return max_sum.ToString();
		}
	}
}
