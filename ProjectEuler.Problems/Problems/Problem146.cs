using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem146 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		private long max_n = 150000000;
		//private long max_n = 1000000;

		private bool IsPrime(long primetest, bool requireExpansion = true)
		{
			if (primetest < 2) return false;
			bool isprime = Primes.IsPrime(primetest, requireExpansion);

			bool isprime_check = true;
			long prime;
			for (int p = 0; isprime_check && (prime = Primes.GetPrime(p)) * prime <= primetest; ++p)
			{
				if (primetest % prime == 0)
				{
					isprime_check = false;
				}
			}

			if (isprime_check != isprime)
			{
				throw new System.Exception("Ick!");
			}

			return isprime_check;
		}

		protected override string CalculateSolution()
		{
			long sum = 0;
			for (long n = 10; n <= max_n; n += 10)
			{
				long n_sq_plus1 = n * n + 1;
				if (
					 n_sq_plus1 % 30 == 11 &&
					 IsPrime(n_sq_plus1, false) &&
					 IsPrime(n_sq_plus1 + 2, false) &&
					 IsPrime(n_sq_plus1 + 6, false) &&
					 IsPrime(n_sq_plus1 + 8, false) &&
					 IsPrime(n_sq_plus1 + 12, false) &&
					 IsPrime(n_sq_plus1 + 26, false) &&
					!IsPrime(n_sq_plus1 + 18, false) &&
					!IsPrime(n_sq_plus1 + 20, false)
					)
				{
					sum += n;
				}
			}

			return sum.ToString();
		}
	}
}
