using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem021 : Problem
	{
		public override string CorrectAnswer { get { return "31626"; } }

		static private BigInteger GetProperDivisors(BigInteger num)
		{
			BigInteger sum = 1;
			int i;

			for (i = 2; i * i < num; i++)
			{
				if (num % i == 0)
				{
					sum += num / i + i;
				}
			}
			if (i * i == num)
				sum += i;

			return sum;
		}

		protected override string CalculateSolution()
		{
			BigInteger sumamicables = 0;
			int next_prime = 0;

			for (int i = 2; i < 10000; i++)
			{
				if (i == Primes.GetPrime(next_prime))
				{
					next_prime++;
				}
				else
				{
					BigInteger testamicable = GetProperDivisors(i);
					if (i != testamicable && i == GetProperDivisors(testamicable))
					{
						sumamicables += i;
					}
				}
			}


			return sumamicables.ToString();
		}
	}
}