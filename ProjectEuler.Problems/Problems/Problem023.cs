using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem023 : Problem
	{
		public override string CorrectAnswer { get { return "4179871"; } }

		private static int analyzedMax = 28123;

		protected override string CalculateSolution()
		{
			int sum = 0;
			List<int> abundantNumbers = new List<int>();
			abundantNumbers.Add(12);
			int minAbundant = abundantNumbers.First();

			for (int numtest = 13;
					abundantNumbers.Last() + minAbundant < analyzedMax;
					++numtest)
			{
				int sumFactors = 1;
				for (int i = 2, iSquared; (iSquared = i * i) <= numtest; ++i)
				{
					if (numtest % i == 0)
					{
						if (iSquared != numtest)
							sumFactors += i + numtest / i;
						else
							sumFactors += i;
					}
				}
				if (sumFactors > numtest)
					if ((numtest & 0x1) == 0)
						abundantNumbers.Add(numtest);
					else
						abundantNumbers.Add(numtest);
			}

			int j = 0, k = abundantNumbers.Count - 1;

			for (int numtest = 1; numtest < analyzedMax; ++numtest)
			{
				bool found = false;
				for (int i = 0, diff; !found && minAbundant <= (diff = numtest - abundantNumbers[i]); ++i)
				{
					if (k <= j)
						k = j + 1;
					while (k < abundantNumbers.Count && abundantNumbers[k] <= diff)
						k = j + 2 * (k - j);
					if (k >= abundantNumbers.Count)
						k = abundantNumbers.Count - 1;

					if (j >= k)
						j = k - 1;
					while (j > 0 && abundantNumbers[j] > diff)
						j = k - 2 * (k - j);
					if (j < 0)
						j = 0;

					while (!(found = abundantNumbers[j] == diff) && k > j + 1)
					{
						if (abundantNumbers[(j + k) / 2] > diff)
							k = (j + k) / 2;
						else
							j = (j + k) / 2;
					}
				}
				if (!found)
					sum += numtest;
			}

			return sum.ToString();
		}
	}
}
