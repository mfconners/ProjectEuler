using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem146 : Problem
	{
		public override string CorrectAnswer { get { return "676333270"; } }

		private int max_n = 150000000;
		//private int max_n = 1000000;

		private bool IsConsecutivePrimes(int n)
		{
			long n_sq = ((long)n) * n;
			if (n_sq % 30 != 10) return false;

			long n_sq_plus1 = n_sq + 1;
			long n_sq_plus3 = n_sq + 3;
			long n_sq_plus7 = n_sq + 7;
			long n_sq_plus9 = n_sq + 9;
			long n_sq_plus13 = n_sq + 13;
			long n_sq_plus27 = n_sq + 27;
			bool has19_factor = false;
			long n_sq_plus19 = n_sq + 19;
			bool has21_factor = false;
			long n_sq_plus21 = n_sq + 21;
			long prime;
			for (int p = 3, max_prime = (int)n + 1; (prime = Primes.GetPrime(p)) <= max_prime; ++p)
			{
				long mod = n_sq_plus27 % prime;
				if (mod == 0) return false;
				if (mod == 14) return false;
				if (mod == 18) return false;
				if (mod == 20) return false;
				if (mod == 24) return false;
				if (mod == 26) return false;
				has19_factor = has19_factor || (mod == 8);
				has21_factor = has21_factor || (mod == 6);
				if (prime < 26)
				{
					if (n_sq_plus1 % prime == 0) return false;
					if (prime < 24)
					{
						if (n_sq_plus3 % prime == 0) return false;
						if (prime < 20)
						{
							if (n_sq_plus7 % prime == 0) return false;
							if (prime < 18)
							{
								if (n_sq_plus9 % prime == 0) return false;
								if (prime < 14)
								{
									if (n_sq_plus13 % prime == 0) return false;
									if (prime < 8)
									{
										has19_factor = has19_factor || (n_sq_plus19 % prime == 0);
										// The following is unnecessary, because the prime testing starts at 7... We've already handled the sieving by this point.
										//if (prime < 6)
										//{
										//	has21_factor = has21_factor || (n_sq_plus21 % prime == 0);
										//}
									}
								}
							}
						}
					}
				}
			}

			return has19_factor && has21_factor;
		}

		protected override string CalculateSolution()
		{
			long sum = 0;
			for (int n = 10; n <= max_n; n += 10)
			{
				if (IsConsecutivePrimes(n))
				{
					sum += n;
				}
			}

			return sum.ToString();
		}
	}
}
