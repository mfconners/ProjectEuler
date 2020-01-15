using ProjectEuler.MathExtensions;
using System.Collections.Generic;

namespace ProjectEuler.Problems
{
	class Problem066 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		const int maxtest_D = 1000;
		protected override string CalculateSolution()
		{
			long max_x = 0;
			long D_for_max_x = 0;
			Queue<(long x, long D, long minus1, long minus_inc, long plus1, long plus_inc)> x_test = new Queue<(long, long, long, long, long, long)>();
			HashSet<long> D_set = new HashSet<long>();

			for (long i = 1, i_sq = 1, D = 2; D <= maxtest_D; ++D)
			{
				while (D > i_sq) { i_sq = ++i * i; }
				if (D != i_sq)
				{
					for (int factor = 1; factor <= D; ++factor)
					{
						long minus_gcd = Ratio.GreatestCommonDivisor(D, factor);
						long plus_gcd = D / minus_gcd;
						if ((factor + 2) % plus_gcd == 0)
						{
							x_test.Enqueue((factor + 1, D, factor / minus_gcd, D / minus_gcd, (factor + 2) / plus_gcd, D / plus_gcd));
						}
					}
					D_set.Add(D);
				}
			}

			(long D, long x) bad_test = (0, 0);
			while (x_test.Count > 0)
			{
				(long x, long D, long minus1, long minus_inc, long plus1, long plus_inc) test = x_test.Dequeue();
				if (!D_set.Contains(test.D))
				{
					continue;
				}
				long x_minus1 = test.minus1;
				long x_plus1 = test.plus1;
				bool still_good = true;
				if (test.D == bad_test.D && test.x == bad_test.x)
				{
					still_good = false;
					x_minus1 = 0;
					x_plus1 = 0;
				}
				else if (x_minus1 % 2 == 0 && x_plus1 % 2 == 0)
				{
					x_minus1 /= 2;
					x_plus1 /= 2;
				}

				int p;
				long prime, prime_sq;
				for (prime_sq = (prime = Primes.GetPrime(p = 0)) * prime; still_good && prime_sq <= x_minus1 && prime_sq <= x_plus1; prime_sq = (prime = Primes.GetPrime(++p)) * prime)
				{
					if (x_minus1 % prime == 0)
					{
						while (x_minus1 % prime_sq == 0)
						{
							x_minus1 /= prime_sq;
						}
						still_good = (x_minus1 % prime != 0);
					}
					else if (x_plus1 % prime == 0)
					{
						while (x_plus1 % prime_sq == 0)
						{
							x_plus1 /= prime_sq;
						}

						if (x_plus1 % prime == 0)
						{
							still_good = false;
							if (prime > 2)
							{
								bad_test.D = test.D;
								bad_test.x = test.x + 2;
							}
						}
					}
				}

				still_good = still_good && (x_minus1 == 1 || x_plus1 == 1);

				for (; still_good && prime_sq <= x_minus1; prime_sq = (prime = Primes.GetPrime(++p)) * prime)
				{
					if (x_minus1 % prime == 0)
					{
						while (x_minus1 % prime_sq == 0)
						{
							x_minus1 /= prime_sq;
						}
						still_good = (x_minus1 % prime != 0);
					}
				}

				for (; still_good && prime_sq <= x_plus1; prime_sq = (prime = Primes.GetPrime(++p)) * prime)
				{
					if (x_plus1 % prime == 0)
					{
						while (x_plus1 % prime_sq == 0)
						{
							x_plus1 /= prime_sq;
						}

						if (x_plus1 % prime == 0)
						{
							still_good = false;
							if (prime > 2)
							{
								bad_test.D = test.D;
								bad_test.x = test.x + 2;
							}
						}
					}
				}

				if (still_good && x_minus1 == 1 && x_plus1 == 1)
				{
					if (test.x > max_x)
					{
						max_x = test.x;
						D_for_max_x = test.D;
					}
					D_set.Remove(test.D);
				}
				else
				{
					test.x += test.D;
					test.minus1 += test.minus_inc;
					test.plus1 += test.plus_inc;
					x_test.Enqueue(test);
				}
			}

			return D_for_max_x.ToString();
		}
	}
}
