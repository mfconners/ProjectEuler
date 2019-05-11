using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem642 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		private const long max_n = 10;

		protected override string CalculateSolution()
		{
			long sum_max_factors = 0;

			for (long n = 2; n <= max_n; ++n)
			{
				long prime_test = n;

				for (int p = 0; !Primes.IsPrime(prime_test, false); ++p)
				{
					long prime = Primes.GetPrime(p, false);
					while (prime_test % prime == 0 && prime_test != prime)
					{
						prime_test /= prime;
					}
				}

				sum_max_factors += prime_test;
				if (sum_max_factors > 1000000000)
					sum_max_factors %= 1000000000;
			}

			return sum_max_factors.ToString();
		}
	}
}
