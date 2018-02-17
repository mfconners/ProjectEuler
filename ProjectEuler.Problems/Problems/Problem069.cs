using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem069 : Problem
	{
		public override string CorrectAnswer { get { return "510510"; } }

		protected override string CalculateSolution()
		{
			int solution = 1;

			for (int p = 0, prime; (prime = Primes.GetPrime(p)) * solution <= 1000000; ++p)
				solution *= prime;

			return solution.ToString();
		}
	}
}
