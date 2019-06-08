using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem518 : Problem
	{
		// Slow: >12 minutes
		public override string CorrectAnswer { get { return "100315739184392"; } }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private void GetFactors(long b, List<long> primeFactors, List<int> primeCounts, List<long> squared_factors)
		{
			long remainder = b + 1;

			for (int p = 0, prime; (prime = Primes.GetPrime(p)) * prime <= remainder; ++p)
			{
				int count = 0;
				while (remainder % prime == 0)
				{
					++count;
					remainder /= prime;
				}

				if (count > 0)
				{
					primeCounts.Add(2 * count);
					primeFactors.Add(prime);
				}
			}

			if (remainder > 1)
			{
				primeFactors.Add(remainder);
				primeCounts.Add(2);
			}

			for (int p = 0; p < primeCounts.Count; ++p)
			{
				int factorCount = squared_factors.Count;
				long primeExponent = 1;
				for (int i = 0; i < primeCounts[p]; ++i)
				{
					primeExponent *= primeFactors[p];
					if (primeExponent < b)
					{
						squared_factors.Add(primeExponent);
						for (int j = 0; j < factorCount; ++j)
						{
							long newFactor = primeExponent * squared_factors[j];
							if (newFactor < b)
							{
								squared_factors.Add(newFactor);
							}
						}
					}
					else
					{
						i = primeCounts[p];
					}
				}
			}

			return;
		}

		private const int n = 100000000;
		//private const int n = 100;
		protected override string CalculateSolution()
		{
			long sum = 0;

			List<long> primeFactors = new List<long>();
			List<int> primeCounts = new List<int>();
			List<long> squared_factors = new List<long>();
			int a_cachedMin = 0, a_cachedMax = 1, c_cachedMin = 0, c_cachedMax = 1;

			long b;
			for (int p_b = 1; (b = Primes.GetPrime(p_b)) < n; ++p_b)
			{
				long bplus1 = b + 1;
				long bplus1_squared = bplus1 * bplus1;
				primeFactors.Clear();
				primeCounts.Clear();
				squared_factors.Clear();
				GetFactors(b, primeFactors, primeCounts, squared_factors);

				foreach (long aplus1 in squared_factors)
				{
					long c = bplus1_squared / aplus1 - 1;
					if (c < n)
					{
						long a = aplus1 - 1;
						if (Primes.IsPrimeWithCache(a, ref a_cachedMin, ref a_cachedMax) && Primes.IsPrimeWithCache(c, ref c_cachedMin, ref c_cachedMax))
						{
							sum += a + b + c;
						}
					}
				}
			}

			return sum.ToString();
		}
	}
}
