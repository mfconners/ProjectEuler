using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem357 : Problem
	{
		// Slow: >2 minutes
		public override string CorrectAnswer { get { return "1739023853137"; } }

		protected override string CalculateSolution()
		{
			long sum = 1 + 2;
			List<long> prime_generators = new List<long>();
			prime_generators.Add(1);
			prime_generators.Add(2);

			for (int p = 2, prime = Primes.GetPrime(p); prime <= 100000001; prime = Primes.GetPrime(++p))
			//for (int p = 2, prime = Primes.GetPrime(p); prime <= 100001; prime = Primes.GetPrime(++p))
			{
				int generator = prime - 1;
				int remainder = generator / 2;

				if (!Primes.IsPrime(remainder + 2))
				{
					continue;
				}

				int pf = 0, pfactor = Primes.GetPrime(pf);
				while (remainder % pfactor != 0 && pfactor * pfactor < remainder)
				{
					++pf;
					pfactor = Primes.GetPrime(pf);
				}

				if (remainder % pfactor == 0 && !Primes.IsPrime(pfactor + generator / pfactor))
				{
					continue;
				}

				for (int factor = pfactor + 1; factor * factor <= generator; ++factor)
				{
					if (generator % factor == 0 && !Primes.IsPrime(factor + generator / factor))
					{
						generator = 0;
						break;
					}
				}

				prime_generators.Add(generator);
				sum += generator;
			}

			return sum.ToString();
		}
	}
}
