using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem035 : Problem
	{
		public override string CorrectAnswer { get { return "55"; } }

		protected override string CalculateSolution()
		{
			int count = 0;

			List<int> digits = new List<int>();
			for (int i = 0; i < 6; ++i)
				digits.Add(0);

			for (int p = 0, prime = 0; (prime = Primes.GetPrime(p)) < 1000000; ++p)
			{
				int test = prime, min = 9, numDigits;
				for (numDigits = 0; test > 0; ++numDigits)
				{
					digits[numDigits] = test % 10;
					if (digits[numDigits] < min)
						min = digits[numDigits];
					test /= 10;
				}

				if (digits[numDigits - 1] == min)
				{
					bool circular = true;
					int tempCount = 1;

					for (int i = 1; circular && i < numDigits; ++i)
					{
						test = 0;
						for (int j = numDigits - 1; j >= 0; --j)
						{
							test *= 10;
							test += digits[(i + j) % numDigits];
						}
						circular = test >= prime && Primes.IsPrime(test);
						if (test != prime)
							++tempCount;
					}

					if (circular)
						count += tempCount;
				}
			}

			return count.ToString();
		}
	}
}
