using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem204 : Problem
	{
		public override string CorrectAnswer { get { return "2944730"; } }

		protected override string CalculateSolution()
		{
			Int64 count = 1;
			List<Int64> nextHamming = new List<Int64>();

			for (int p = 0, prime; (prime = Primes.GetPrime(p)) <= 100; ++p)
				nextHamming.Add(prime);

			int i = nextHamming.Count - 1;
			while (i >= 0)
			{
				if (nextHamming[i] <= 1000000000)
				{
					++count;
					Int64 hamming = nextHamming[i];
					for (int j = i; j < nextHamming.Count && (nextHamming[j] = hamming * Primes.GetPrime(j)) <= 1000000000; ++j)
						i = j;
				}
				else
					--i;
			}

			return count.ToString();
		}
	}
}
