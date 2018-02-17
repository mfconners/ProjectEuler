using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem057 : Problem
	{
		public override string CorrectAnswer { get { return "153"; } }

		protected override string CalculateSolution()
		{
			int count = 0;
			double prevNumerator = 10.0, prevDenominator = 0.0;
			double numerator = 10.0, denominator = 10.0;

			for (int i = 0; i < 1000; ++i)
			{
				if (numerator > 100.0)
				{
					denominator /= 10.0;
					numerator /= 10.0;
					prevDenominator /= 10.0;
					prevNumerator /= 10.0;
				}

				if (denominator < 10.0)
					++count;

				double temp = numerator;
				numerator *= 2;
				numerator += prevNumerator;
				prevNumerator = temp;

				temp = denominator;
				denominator *= 2;
				denominator += prevDenominator;
				prevDenominator = temp;
			}

			return count.ToString();
		}
	}
}