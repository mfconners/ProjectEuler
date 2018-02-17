using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem050 : Problem
	{
		public override string CorrectAnswer { get { return "997651"; } }

		private static readonly BigInteger oneMillion = 1000000;

		protected override string CalculateSolution()
		{
			//int millionPrimeIndex = 0;
			int start, count, maxCount = 0;
			int bigPrime = 0, sum = 0;

			for (count = 0, sum = 0;
					sum < oneMillion;
					sum += Primes.GetPrime(count++), sum += Primes.GetPrime(count++))
			{
				if (Primes.IsPrime(sum))
				{
					maxCount = count;
					bigPrime = sum;
				}
			}

			for (start = 1, count = maxCount + 1; maxCount < count; start++)
			{
				for (count = 1, sum = Primes.GetPrime(start);
						sum < oneMillion;
						sum += Primes.GetPrime(start + count++), sum += Primes.GetPrime(start + count++))
				{
					if (count > maxCount && Primes.IsPrime(sum))
					{
						maxCount = count;
						bigPrime = sum;
					}
				}
			}

			return bigPrime.ToString();
		}
	}
}
