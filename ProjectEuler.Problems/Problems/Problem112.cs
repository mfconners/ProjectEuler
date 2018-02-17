using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem112 : Problem
	{
		public override string CorrectAnswer { get { return "1587000"; } }

		protected override string CalculateSolution()
		{
			int num = 100, bouncyCount = 0;
			List<int> digits = new List<int>();
			digits.Add(0);
			digits.Add(0);
			digits.Add(1);

			while (digits[0] != 0 || digits[1] != 0 || bouncyCount * 100 != num * 99)
			{
				++num;
				++digits[0];
				for (int i = 0; digits[i] >= 10; ++i)
				{
					if (i + 1 < digits.Count)
						digits[i + 1] += digits[i] / 10;
					else
						digits.Add(digits[i] / 10);
					digits[i] %= 10;
				}

				bool increasing = false, decreasing = false;
				for (int i = digits.Count - 1; i > 0 && (!increasing || !decreasing); --i)
				{
					if (digits[i] < digits[i - 1])
						increasing = true;
					else if (digits[i] > digits[i - 1])
						decreasing = true;
				}
				if (increasing && decreasing)
					++bouncyCount;
			}

			return num.ToString();
		}
	}
}
