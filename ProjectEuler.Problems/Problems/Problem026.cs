using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem026 : Problem
	{
		public override string CorrectAnswer { get { return "983"; } }

		protected override string CalculateSolution()
		{
			Dictionary<int, int> sequence = new Dictionary<int, int>(1000);
			int maxprime = 0, maxcount = 0;

			for (int p = Primes.IndexOfPrimeAtMost(1000), prime;
					(prime = Primes.GetPrime(p)) > maxcount;
					--p)
			{
				sequence.Clear();
				int remainder = 1;
				for (int i = 0; !sequence.ContainsKey(remainder); ++i, remainder = 10 * remainder % prime)
					sequence.Add(remainder, i);
				if (sequence.Count - sequence[remainder] > maxcount)
				{
					maxprime = prime;
					maxcount = sequence.Count - sequence[remainder];
				}
			}

			return maxprime.ToString();
		}
	}
}
