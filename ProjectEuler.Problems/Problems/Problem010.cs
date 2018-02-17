using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem010 : Problem
	{
		public override string CorrectAnswer { get { return "142913828922"; } }

		const int max_prime_test = 2000000;

		protected override string CalculateSolution()
		{
			long sum = 0;

			for (int p = 0, prime; (prime = Primes.GetPrime(p)) <= max_prime_test; ++p)
			{
				sum += prime;
			}

			return sum.ToString();
		}
	}
}
