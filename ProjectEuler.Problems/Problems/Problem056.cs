using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem056 : Problem
	{
		public override string CorrectAnswer { get { return "972"; } }

		protected override string CalculateSolution()
		{
			Int64 maxDigitalSum = 0;
			List<Int64> digits = new List<Int64>();

			for (Int64 a = 99; a >= 0; --a)
			{
				digits.Clear();
				digits.Add(1);
				Int64 digitalSum = 1;
				for (Int64 b = 1; b < 100; ++b)
				{
					Int64 remainder = 0;
					for (int d = 0; d < digits.Count(); ++d)
					{
						digitalSum -= digits[d];
						digits[d] *= a;
						digits[d] += remainder;
						remainder = digits[d] / 10;
						digits[d] %= 10;
						if (remainder > 0 && d + 1 >= digits.Count)
							digits.Add(0);
						digitalSum += digits[d];
					}
					if (digitalSum > maxDigitalSum)
						maxDigitalSum = digitalSum;
				}
			}

			return maxDigitalSum.ToString();
		}
	}
}
