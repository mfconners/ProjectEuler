using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem034 : Problem
	{
		public override string CorrectAnswer { get { return "40730"; } }

		protected override string CalculateSolution()
		{
			int theSum = 0;

			List<int> digitFactorial = new List<int>(10);
			for (int i = 0, factorial = 1; i < 10; factorial *= ++i)
				digitFactorial.Add(factorial);

			int test = 10;
			int maxSum = digitFactorial[9] + digitFactorial[9];
			int minTest = 10;
			List<int> digits = new List<int>();
			digits.Add(0);
			digits.Add(1);
			int testSum = digitFactorial[0] + digitFactorial[1];

			while (minTest <= maxSum)
			{
				if (test == testSum)
					theSum += testSum;

				++test;
				for (int i = 0, remainder = 10; i == 0 || digits[i - 1] == 0; ++i, remainder *= 10)
				{
					if (i < digits.Count)
					{
						if (testSum > test && digits[i] != 9)
						{
							testSum -= digitFactorial[digits[i]];
							digits[i] = 0;
							test = (test / remainder + 1) * remainder;
						}
						else
						{
							testSum -= digitFactorial[digits[i]];
							digits[i] = (digits[i] + 1) % 10;
						}
					}
					else
					{
						digits.Add(1);
						minTest *= 10;
						maxSum += digitFactorial[9];
					}

					testSum += digitFactorial[digits[i]];
				}
			}

			return theSum.ToString();
		}
	}
}
