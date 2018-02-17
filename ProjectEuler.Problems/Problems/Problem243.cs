using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem243 : Problem
	{
		public override string CorrectAnswer { get { return "892371480"; } }

		protected override string CalculateSolution()
		{
			long denominator = 1;
			long count = 1;
			for (int i = 0, prime = Primes.GetPrime(0);
					15499 * prime * (denominator - 1) < 94744 * count * (prime - 1);
					prime = Primes.GetPrime(++i))
			{
				denominator *= prime;
				count *= prime - 1;
			}

			while (15499 * (denominator - 1) < 94744 * count)
			{
				denominator *= 2;
				count *= 2;
			}

			return denominator.ToString();
		}
	}
}
