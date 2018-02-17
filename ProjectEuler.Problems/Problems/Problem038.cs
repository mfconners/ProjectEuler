using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem038 : Problem
	{
		public override string CorrectAnswer { get { return "932718654"; } }

		protected override string CalculateSolution()
		{
			int maximum;

			// Of the 1-digit numbers, only 4 does not align with a 9 digit number when multiplied
			maximum = GetMaxInTestRange(5, 9); // 5 to 9
			maximum = Max(maximum, GetMaxInTestRange(1, 3)); // 1 to 3

			// The range of numbers where 1, 2 and 3 times the number have 2 bigDigits AND
			// 4 times the number has three bigDigits
			// These are the only 2-digit numbers that work...
			// Max should be 99 / 3 = 33, but anything more than 32 can not be pan-digital.
			maximum = Max(maximum, GetMaxInTestRange(100 / 4, 99 / 3));

			// The range of numbers where 1, 2 and 3 times the number have 3 bigDigits
			// These are the only 3-digit numbers that work...
			// Min should be 1000 / 10 = 100, but anything less than 123 can not be pan-digital.
			// Max should be 999 / 3 = 333, but anything more than 321 can not be pan-digital.
			maximum = Max(maximum, GetMaxInTestRange(123, 321));

			// The range of numbers where 1 times the number has 4 bigDigits AND
			// 2 times the number has five bigDigits
			// These are the only 4-digit numbers that work...
			// Min should be 10000 / 2 = 5000, but anything less than 5321 can not be pan-digital
			// Max should be 9999 / 1 = 9999, but anything more than 9876 can not be pan-digital.
			maximum = Max(maximum, GetMaxInTestRange(5321, 9876));

			return maximum.ToString();
		}

		private static int Max(int a, int b)
		{
			if (a >= b)
				return a;
			else
				return b;
		}

		private static int GetMaxInTestRange(int minTest, int maxTest)
		{
			int maximum = 0;

			List<int> digits = new List<int>(9);
			List<int> starts = new List<int>();
			BitArray usedDigits = new BitArray(10);

			for (int num = maxTest; num >= minTest; --num)
			{
				digits.Clear();
				starts.Clear();
				usedDigits.SetAll(false);

				for (int n = 1, nextDigit = num; digits.Count < 9 && !usedDigits[0]; ++n, nextDigit = n * num)
				{
					starts.Add(digits.Count);
					digits.Add(nextDigit);

					int lastDigit;
					for (lastDigit = digits.Last();
							!usedDigits[0] && digits.Count < 9 && lastDigit >= 10;
							lastDigit = digits.Last())
					{
						nextDigit = lastDigit / 10;
						lastDigit %= 10;
						digits[digits.Count - 1] = lastDigit;
						if (!usedDigits[lastDigit])
							usedDigits[lastDigit] = true;
						else
						{
							usedDigits[0] = true;
							break;
						}
						digits.Add(nextDigit);
					}

					if (lastDigit < 10 && !usedDigits[lastDigit])
						usedDigits[lastDigit] = true;
					else
					{
						usedDigits[0] = true;
						break;
					}
				}

				if (!usedDigits[0] && digits.Last() < 10)
				{
					starts.Add(9);
					int testMax = 0;
					for (int i = 0, multiplier = 100000000; i + 1 < starts.Count; ++i)
					{
						for (int j = starts[i + 1] - starts[i] - 1; j >= 0; --j)
						{
							testMax += digits[starts[i] + j] * multiplier;
							multiplier /= 10;
						}
					}
					maximum = Max(maximum, testMax);

					int range = 100000000;
					while (num % range == num)
						range /= 10;
					num = range;
				}
			}

			return maximum;
		}
	}
}
