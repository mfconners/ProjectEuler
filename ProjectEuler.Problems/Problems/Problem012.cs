using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem012 : Problem
	{
		public override string CorrectAnswer { get { return "76576500"; } }

		protected override string CalculateSolution()
		{
			BigInteger number = 0, triangleNumber = 0, testNumber = 0;
			int i = 0, curCount = 0, totalCount = 0;

			while (totalCount < 500)
			{
				number++;
				if (triangleNumber + number <= triangleNumber)
				{
					return "triangleNumber is too big!";
				}

				triangleNumber += number;
				curCount = totalCount = 1;
				for (i = 0, testNumber = triangleNumber;
						testNumber / Primes.GetPrime(i) >= Primes.GetPrime(i);
						i++)
				{
					for (curCount = 1;
							testNumber % Primes.GetPrime(i) == 0;
							curCount++, testNumber /= Primes.GetPrime(i))
						;
					totalCount *= curCount;
				}

				if (testNumber > 1)
					totalCount *= 2;
			}

			return triangleNumber.ToString();
		}
	}
}
