using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem043 : Problem
	{
		public override string CorrectAnswer { get { return "16695334890"; } }

		private static int[] testOrder = { 3, 5, 2, 4, 6, 7, 8, 9, 0, 1 };
		private static int[] increment = { 1, 1, 1, 2, 3, 5, 7, 11, 13, 17 };
		private const int max = 2 * 3 * 5 * 7 * 11 * 13 * 17;

		protected override string CalculateSolution()
		{
			long sum = 0;
			List<int> digits = new List<int>(10);
			for (int i = 0; i < 10; ++i)
				digits.Add(-1);

			int level = 0;
			bool next = false;
			while (level >= 0)
			{
				if (level == 10)
				{
					long multiplier = 1;
					for (int i = 9; i >= 0; --i, multiplier *= 10)
					{
						sum += multiplier * digits[i];
					}
					if (digits[testOrder[8]] < digits[testOrder[9]])
					{
						int temp = digits[testOrder[8]];
						digits[testOrder[8]] = digits[testOrder[9]];
						digits[testOrder[9]] = temp;
					}
					else
					{
						digits[testOrder[8]] = -1;
						digits[testOrder[9]] = -1;
						next = true;
						level -= 3;
					}
				}
				else
				{
					int i_digit = testOrder[level];

					if (digits[i_digit] > 9)
					{
						digits[i_digit] = -1;
						--level;
						next = true;
					}
					else if (digits[i_digit] < 0)
					{
						switch (i_digit)
						{
							case 0:
								digits[i_digit] = 1;
								break;
							case 1:
								digits[i_digit] = 0;
								break;
							default:
								digits[i_digit] = (max - 100 * digits[i_digit - 2] - 10 * digits[i_digit - 1]) % increment[i_digit];
								break;
						}
						next = false;
					}
					else if (next == true)
					{
						digits[i_digit] += increment[i_digit];
						next = false;
					}
					else
					{
						for (int i = 0; i < level && digits[i_digit] >= 0; ++i)
						{
							if (digits[i_digit] == digits[testOrder[i]])
							{
								next = true;
								break;
							}
						}

						if (!next)
							++level;
					}
				}
			}

			return sum.ToString();
		}
	}
}
