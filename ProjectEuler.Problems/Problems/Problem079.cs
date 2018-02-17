using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem079 : Problem
	{
		public override string CorrectAnswer { get { return "73162890"; } }

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			StringBuilder solution = new StringBuilder(10);
			Dictionary<char, int> digit_count = new Dictionary<char, int>();
			Dictionary<char, List<string>> lead_digit = new Dictionary<char, List<string>>();

			// Read all lines into memory.
			// digit_count is the count of occurrences for each digit in the total set of entered codes.
			// lead_digit is the remainder of codes starting with each digit.
			string[] keylog_file = Properties.Resources.keylog.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);
			foreach(string line in keylog_file)
			{
				for (int i = 0; i < line.Length; ++i)
				{
					if (!digit_count.ContainsKey(line[i]))
						digit_count.Add(line[i], 1);
					else
						++digit_count[line[i]];
				}

				if (line.Length > 0)
				{
					if (!lead_digit.ContainsKey(line[0]))
						lead_digit.Add(line[0], new List<string>());
					lead_digit[line[0]].Add(line.Substring(1));
				}
			}

			// This only works if there are no repeated digits in the shortest possible code...
			while (digit_count.Count > 0)
			{
				foreach (char key in lead_digit.Keys)
				{
					if (digit_count[key] == lead_digit[key].Count)
					{
						solution.Append(key);
						foreach (string remainder in lead_digit[key])
						{
							if (remainder.Length > 0)
							{
								if (!lead_digit.ContainsKey(remainder[0]))
									lead_digit.Add(remainder[0], new List<string>());
								lead_digit[remainder[0]].Add(remainder.Substring(1));
							}
						}
						digit_count.Remove(key);
						lead_digit.Remove(key);
						break;
					}
				}
			}

			return solution.ToString();
		}
	}
}
