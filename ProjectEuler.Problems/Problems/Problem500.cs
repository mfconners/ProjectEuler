using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem500 : Problem
	{
		public override string CorrectAnswer { get { return "35407281"; } }

		protected override string CalculateSolution()
		{
			int p = 0;
			long next_prime = Primes.GetPrime(p);

			long factor = 1;
			HashSet<long> next_factors = new HashSet<long>();
			
			long product = 1;
			
			for (int factor_count = 500500; factor_count > 0; --factor_count)
			{
				while (factor != next_prime && !next_factors.Contains(factor)) ++factor;

				product *= factor;
				product %= 500500507;
				next_factors.Add(factor * factor);
				if (factor == next_prime)
				{
					next_prime = Primes.GetPrime(++p);
				}
				else
				{
					next_factors.Remove(factor);
				}

			}

			return product.ToString();
		}
	}
}
