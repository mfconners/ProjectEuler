using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem053 : Problem
	{
		public override string CorrectAnswer { get { return "4075"; } }

		protected override string CalculateSolution()
		{
			BigInteger totalCount = 0;
			for (BigInteger num = 23; num <= 100; ++num)
			{
				BigInteger productnum = 1;
				BigInteger productden = 1;
				for (BigInteger den = 1; den <= num / 2; ++den)
				{
					productnum *= num - den + 1;
					productden *= den;
					BigInteger prime;
					for (int p = 0; (prime = Primes.GetPrime(p)) <= den; ++p)
						if (productnum % prime == 0 && productden % prime == 0)
						{
							productnum /= prime;
							productden /= prime;
						}
					if (productnum / productden > 1000000)
					{
						totalCount += num - 2 * den + 1;
						break;
					}
				}
			}

			return totalCount.ToString();
		}
	}
}
