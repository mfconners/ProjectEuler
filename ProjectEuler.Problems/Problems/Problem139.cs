using ProjectEuler.MathExtensions;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem139 : Problem
	{
		public override string CorrectAnswer { get { return "10057761"; } }

		private const long max_perimeter = 100000000;
		private const long half_max_perimeter = max_perimeter / 2;

		protected override string CalculateSolution()
		{
			int count = 0;
			List<long> prime_factors = new List<long>();
			// a = k * (m^2 - n^2);
			// b = k * (2 * m * n);
			// c = k * (m^2 + n^2);
			// min_perimeter = 4*n^2 + 6*n + 2;
			for (long n = 1; 4 * n * n + 6 * n <= max_perimeter; ++n)
			{
				prime_factors.Clear();
				long remainder = n;
				while (remainder % 2 == 0) remainder /= 2;
				for (int p = 0; remainder > 1; ++p)
				{
					long prime = Primes.GetPrime(p);
					if (remainder % prime == 0)
					{
						remainder /= prime;
						prime_factors.Add(prime);
						while (remainder % prime == 0) remainder /= prime;
					}

					if (prime * prime > remainder && remainder > 1)
					{
						prime_factors.Add(remainder);
						remainder = 1;
					}
				}

				for (long m = n + 1, perimeter; (perimeter = 2 * m * (m + n)) < max_perimeter; m += 2)
				{
					bool mn_coprime = true;
					for (int p_test = 0; p_test < prime_factors.Count; ++p_test)
					{
						if (m % prime_factors[p_test] == 0)
						{
							mn_coprime = false;
							break;
						}
					}

					if (mn_coprime)
					{
						long a = m * m - n * n;
						long b = 2 * m * n;
						long c = m * m + n * n;

						if (c % (a - b) == 0)
						{
							for (int k = 1; k * perimeter < max_perimeter; ++k)
								++count;
						}
					}
				}
			}

			return count.ToString();
		}
	}
}
