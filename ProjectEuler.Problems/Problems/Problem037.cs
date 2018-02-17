using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem037 : Problem
	{
		public override string CorrectAnswer { get { return "748317"; } }

		private static int oneMillion = 1000000;

		protected override string CalculateSolution()
		{
			int sum = 0;
			for (int i = 0, prime; (prime = Primes.GetPrime(i)) < oneMillion; ++i)
			{
				bool isCircular = false;
				for (int p = prime / 10, mod = 10;
						(mod < prime) &&
						(isCircular = (Primes.IsPrime(p) && Primes.IsPrime(prime % mod)));
						p /= 10, mod *= 10)
					;
				if (isCircular)
					sum += prime;
			}

			return sum.ToString();
		}
	}
}
