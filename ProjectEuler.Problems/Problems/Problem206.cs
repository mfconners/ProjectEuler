using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem206 : Problem
	{
		public override string CorrectAnswer { get { return "1389019170"; } }

		protected override string CalculateSolution()
		{
			ulong predefined = 0;
			for (ulong digit = 9, pos = 1; digit > 0; --digit, pos *= 100)
			{
				predefined += digit * pos;
			}

			List<ulong> positions = new List<ulong>();
			List<ulong> digits = new List<ulong>();
			for (ulong i = 0, pos = 10; i < 8; ++i, pos *= 100)
			{
				positions.Add(pos);
				digits.Add(0);
			}

			ulong test = 100000000;
			ulong test_squared = test * test;

			while (true)
			{
				#region Build the current guess square
				ulong square = predefined;
				for (int i = 0, adder = 0; i < digits.Count; ++i)
				{
					if (adder > 0)
					{
						digits[i] += positions[i];
						adder = 0;
					}
					if (digits[i] >= 10 * positions[i])
					{
						square = predefined;
						adder = 1;
						for (int j = i; j >= i; --j)
							digits[j] = 0;
					}
					else
						square += digits[i];
				}
				#endregion

				#region Find the first square at least as big as the guess...
				if (test_squared < square)
				{
					ulong max = 2 * test;
					while (max * max < square)
						max *= 2;
					while (test_squared <= square)
					{
						ulong mid = (test + max) / 2;

						if (mid == test)
						{
							++test;
							test_squared = test * test;
						}
						else
						{
							ulong mid_squared = mid * mid;
							if (mid_squared <= square)
							{
								test = mid;
								test_squared = mid_squared;
							}
							else
							{
								max = mid;
							}
						}
					}
				}
				#endregion

				if (test_squared == square)
				{
					return test.ToString() + "0";
				}
				else
				{
					ulong diff = test_squared - square;
					if (diff < 10)
					{
						digits[0] += 10;
					}
					else
					{
						int pos = 0;

						for (int i = 0; i < positions.Count && square + positions[i] < test_squared; ++i)
							pos = i;

						digits[pos] += diff / positions[pos] * positions[pos];
					}
				}
			}
		}
	}
}
