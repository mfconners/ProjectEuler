using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem146 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		private long max_n = 150000000;
		//private long max_n = 1000000;

		protected override string CalculateSolution()
		{
			long sum = 0;
			for (long n = 10; n <= max_n; n += 10)
			{
				if (n % 30 == 0) continue;

				long n_sq_plus1 = n * n + 1;
				if (
					 Primes.IsPrime(n_sq_plus1, false) &&
					 Primes.IsPrime(n_sq_plus1 + 2, false) &&
					 Primes.IsPrime(n_sq_plus1 + 6, false) &&
					 Primes.IsPrime(n_sq_plus1 + 8, false) &&
					 Primes.IsPrime(n_sq_plus1 + 12, false) &&
					 Primes.IsPrime(n_sq_plus1 + 26, false) &&
					!Primes.IsPrime(n_sq_plus1 + 18, false) &&
					!Primes.IsPrime(n_sq_plus1 + 20, false)
					)
				{
					sum += n;
				}
			}

			return sum.ToString();
		}
	}
}
