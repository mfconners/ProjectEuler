using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem072 : Problem
	{
		public override string CorrectAnswer { get { return "303963552391"; } }

		int oneMillion = 1000000;

		protected override string CalculateSolution()
		{
			long count = 0;

			for (int denominator = 2; denominator <= oneMillion; ++denominator)
			{
				if (Primes.IsPrime(denominator))
				{
					count += denominator - 1;
				}
				else
				{
					int curCount = 1, factored = denominator;
					for (int i = 0, prime; (prime = Primes.GetPrime(i)) * prime <= factored; ++i)
					{
						if (factored % prime == 0)
						{
							curCount *= prime - 1;
							factored /= prime;
							while (factored % prime == 0)
							{
								curCount *= prime;
								factored /= prime;
							}
						}
					}
					if (factored > 1)
						curCount *= factored - 1;

					count += curCount;
				}
			}

			return count.ToString();
		}
	}
}
