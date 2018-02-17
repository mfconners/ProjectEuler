using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem040 : Problem
	{
		public override string CorrectAnswer { get { return "210"; } }

		protected static int GetNthDigit(int n)
		{
			int magnitudeNum = 1;
			int magnitudeCount = 9;
			int magnitude = 1;

			n -= 1;
			while (n >= magnitudeNum * magnitudeCount)
			{
				n -= magnitudeNum * magnitudeCount;
				++magnitudeNum;
				magnitudeCount *= 10;
				magnitude *= 10;
			}

			int number = n / magnitudeNum + magnitude;
			int digit = n % magnitudeNum;

			while (digit + 1 < magnitudeNum)
			{
				++digit;
				number /= 10;
			}

			return number % 10;
		}

		protected override string CalculateSolution()
		{
			int product = 1;
			for (int d = 1; d <= 1000000; d *= 10)
				product *= GetNthDigit(d);

			return product.ToString();
		}
	}
}
