using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem027 : Problem
	{
		public override string CorrectAnswer { get { return "-59231"; } }

		const int oneThousand = 1000;

		protected override string CalculateSolution()
		{
			long nMax = 0;
			long maxProduct = 0;

			for (int i = 1, b; (b = Primes.GetPrime(i)) < oneThousand; ++i)
			{
				for (int a = -999; a < 1000; ++a)
				{
					int n = 1;
					while (Primes.IsPrime(n * n + a * n + b))
						++n;
					if (n > nMax)
					{
						maxProduct = a * b;
						nMax = n;
					}
				}
			}
			return maxProduct.ToString();
		}
	}
}
