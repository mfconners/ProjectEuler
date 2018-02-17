using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem003 : Problem
	{
		public override string CorrectAnswer { get { return "6857"; } }

		protected override string CalculateSolution()
		{
			long bignumber = 600851475143;

			{
				long p;
				for (int i = 0;
						(p = Primes.GetPrime(i)) * p <= bignumber;
						i++)
				{
					while (bignumber % p == 0 && bignumber > p)
					{
						bignumber /= p;
					}
				}
			}

			return bignumber.ToString();
		}
	}
}
