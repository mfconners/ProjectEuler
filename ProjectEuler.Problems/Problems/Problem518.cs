using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem518 : Problem
	{
		// Slow: >13 minutes
		public override string CorrectAnswer { get { return "100315739184392"; } }

		static private List<long> GetFactors(long b)
		{
			// TODO Garbage Collection: Allocating at a high rate?
			List<long> primeFactors = new List<long>();
			List<int> primeCounts = new List<int>();
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

			// TODO Garbage Collection: Allocating at a high rate?
			List<long> factors = new List<long>();
			for (int p = 0; p < primeCounts.Count; ++p)
			{
				int factorCount = factors.Count;
				long primeExponent = 1;
				for (int i = 0; i < primeCounts[p]; ++i)
				{
					primeExponent *= primeFactors[p];
					if (primeExponent < b)
					{
						factors.Add(primeExponent);
						for (int j = 0; j < factorCount; ++j)
						{
							long newFactor = primeExponent * factors[j];
							if (newFactor < b)
							{
								factors.Add(newFactor);
							}
						}
					}
					else
					{
						i = primeCounts[p];
					}
				}
			}

			return factors;
		}

		private const int n = 100000000;
		//private const int n = 100;
		protected override string CalculateSolution()
		{
			long sum = 0;

			long b;
			for (int p_b = 1; (b = Primes.GetPrime(p_b)) < n; ++p_b)
			{
				long bplus1_squared = (b + 1) * (b + 1);
				List<long> factors = GetFactors(b);

				foreach (long aplus1 in factors)
				{
					long c = bplus1_squared / aplus1 - 1;
					if (c < n)
					{
						long a = aplus1 - 1;
						if (Primes.IsPrime(a) && Primes.IsPrime(c))
							sum += a + b + c;
					}
				}
			}

			return sum.ToString();
		}
	}
}
