using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem089 : Problem
	{
		public override string CorrectAnswer { get { return "743"; } }

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			int count = 0;

			Dictionary<char, int> romanNumerals = new Dictionary<char, int>();
			romanNumerals.Add('I', 1);
			romanNumerals.Add('V', 5);
			romanNumerals.Add('X', 10);
			romanNumerals.Add('L', 50);
			romanNumerals.Add('C', 100);
			romanNumerals.Add('D', 500);
			romanNumerals.Add('M', 1000);

			string[] roman_file = Properties.Resources.roman.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in roman_file)
			{
				int sum = 0;
				for (int i = 0; i < line.Length; ++i)
				{
					if (romanNumerals.ContainsKey(line[i]))
					{
						int digit = romanNumerals[line[i]];
						if (i + 1 < line.Length &&
								romanNumerals.ContainsKey(line[i + 1]) &&
								digit < romanNumerals[line[i + 1]])
						{
							sum -= digit;
						}
						else
						{
							sum += digit;
						}
					}
				}

				count -= sum / 1000;
				sum %= 1000;

				for (int div10 = 1000, div5 = 500, div1 = 100; sum > 0; div10 = div1, div1 /= 10, div5 /= 10)
				{
					if (sum >= div5 - div1)
					{
						if (sum >= div10 - div1)
						{
							count -= 2;
							sum -= div10 - div1;
						}
						else if (sum >= div5)
						{
							--count;
							sum -= div5;
						}
						else
						{
							count -= 2;
							sum -= div5 - div1;
						}
					}

					count -= sum / div1;
					sum %= div1;
				}

				count += line.Length;
			}

			return count.ToString();
		}
	}
}
