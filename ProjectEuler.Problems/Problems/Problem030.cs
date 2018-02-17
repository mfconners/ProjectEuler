using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem030 : Problem
	{
		public override string CorrectAnswer { get { return "443839"; } }

		protected override string CalculateSolution()
		{
			int sum = 0;

			List<int> fifthPower = new List<int>(10);
			for (int i = 0; i <= 10; ++i)
				fifthPower.Add(i * i * i * i * i);

			int num = 1;
			List<int> digits = new List<int>();
			digits.Add(1);
			int digitSum = fifthPower[1];
			int maxSum = 2 * fifthPower[9];

			while (num < maxSum)
			{
				if (digitSum > num)
				{
					for (int i = 0, orderOfMagnitude = 10; i < digits.Count && digitSum > num; ++i, orderOfMagnitude *= 10)
					{
						digitSum -= fifthPower[digits[i]];
						digits[i] = 0;

						if (i + 1 < digits.Count)
						{
							digitSum -= fifthPower[digits[i + 1]];
							digits[i + 1] = (digits[i + 1] + 1) % 10;
							digitSum += fifthPower[digits[i + 1]];

							while (digits[i + 1] == 0)
							{
								++i;
								if (i + 1 < digits.Count)
								{
									digitSum -= fifthPower[digits[i + 1]];
									digits[i + 1] = (digits[i + 1] + 1) % 10;
									digitSum += fifthPower[digits[i + 1]];
								}
								else
								{
									digits.Add(1);
									digitSum += fifthPower[1];
									maxSum += fifthPower[9];
								}
							}
						}
						else
						{
							digits.Add(1);
							digitSum += fifthPower[1];
							maxSum += fifthPower[9];
						}
						num = num - num % orderOfMagnitude + orderOfMagnitude;
					}
				}
				else
				{
					for (int i = 0; i == 0 || digits[i - 1] == 0; ++i)
					{
						if (i < digits.Count)
						{
							digitSum -= fifthPower[digits[i]];
							digits[i] = (digits[i] + 1) % 10;
							digitSum += fifthPower[digits[i]];
						}
						else
						{
							digits.Add(1);
							digitSum += fifthPower[1];
							maxSum += fifthPower[9];
						}
					}
					++num;
				}

				if (num >= 69444)
				{
					if (true) { }
				}

				if (num == digitSum)
				{
					sum += num;
				}
			}

			return sum.ToString();
		}
	}
}
