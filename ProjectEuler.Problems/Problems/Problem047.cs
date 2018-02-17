using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem047 : Problem
	{
		public override string CorrectAnswer { get { return "134043"; } }

		protected override string CalculateSolution()
		{
			double first = Primes.GetPrime(0) * Primes.GetPrime(1);
			for (int i = 0; i < 14; ++i)
				first *= Primes.GetPrime(i);
			first = Math.Pow(first, .25);
			first = Math.Ceiling(first);

			long initMax = Primes.GetPrime(0) * Primes.GetPrime(1) *
					Primes.GetPrime(2) * Primes.GetPrime(3);

			int found = 0;
			long test;
			for (test = (long)first; found < 4; ++test)
			{
				long max = initMax;
				int factors = 0;
				long remainder = test;
				for (int j = 0; remainder >= max && factors < 3; ++j)
				{
					int prime = Primes.GetPrime(j);
					if (remainder % prime == 0)
					{
						do
							remainder /= prime;
						while (remainder % prime == 0);
						++factors;
					}
					else
					{
						max *= Primes.GetPrime(j + 4 - factors);
					}
					max /= prime;
				}
				if (factors >= 3 && remainder > 1)
					++found;
				else
					found = 0;
			}

			return (test - 4).ToString();
		}
	}
}
