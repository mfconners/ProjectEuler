using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem005 : Problem
	{
		public override string CorrectAnswer { get { return "232792560"; } }

		protected override string CalculateSolution()
		{
			long product = 1;
			for (int p = 0, prime; (prime = Primes.GetPrime(p)) <= 20; ++p)
			{
				long prime_power = prime;
				while (prime_power * prime <= 20)
				{
					prime_power *= prime;
				}
				product *= prime_power;
			}

			return product.ToString();
		}
	}
}
