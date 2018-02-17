using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem073 : Problem
	{
		public override string CorrectAnswer { get { return "7295372"; } }

		protected override string CalculateSolution()
		{
			int count = 0;

			for (int d = 2; d <= 12000; ++d)
			{
				int max_n = (d - 1) / 2;
				for (int n = d / 3 + 1; n <= max_n; ++n)
				{
					if (d % n != 0)
					{
						bool found_common_factor = false;
						for (int p = 0, prime, n_temp = n, d_temp = d; (prime = Primes.GetPrime(p)) * prime <= n_temp && !found_common_factor; ++p)
						{
							if (n_temp % prime == 0 && d_temp % prime == 0)
							{
								found_common_factor = true;
							}
							else
							{
								while (n_temp % prime == 0)
									n_temp /= prime;
								while (d_temp % prime == 0)
									d_temp /= prime;
								if (d_temp % n_temp == 0 && n_temp > 1)
									found_common_factor = true;
							}
						}

						if (!found_common_factor)
						{
							++count;
						}
					}
				}
			}

			return count.ToString();
		}
	}
}
