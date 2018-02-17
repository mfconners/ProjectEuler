using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	abstract class MaximumPathSum : Problem
	{
		static private long maximum(long a, long b)
		{
			if (a > b)
				return a;
			else
				return b;
		}

		protected abstract string Triangle { get; }

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			List<long> oddrow = new List<long>(), evenrow = new List<long>();
			string[] triangle_file = Triangle.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);

			long biggest = 0;

			foreach (string nextline in triangle_file)
			{
				oddrow.Add(0);
				evenrow.Add(0);
				int i = oddrow.Count;

				if (i % 2 == 0)
				{
					for (int j = 0; j < i; j++)
					{
						if (nextline.Length > 3 * j + 1)
						{
							evenrow[j] = 10 * (nextline[3 * j] - '0') + nextline[3 * j + 1] - '0';
							if (j > 0)
							{
								evenrow[j] += maximum(oddrow[j - 1], oddrow[j]);
							}
							else
							{
								evenrow[j] += oddrow[j];
							}
							if (evenrow[j] > biggest)
								biggest = evenrow[j];
						}
					}
				}
				else
				{
					for (int j = 0; j < i; j++)
					{
						if (nextline.Length > 3 * j + 1)
						{
							oddrow[j] = 10 * (nextline[3 * j] - '0') + nextline[3 * j + 1] - '0';
							if (j > 0)
							{
								oddrow[j] += maximum(evenrow[j - 1], evenrow[j]);
							}
							else
							{
								oddrow[j] += evenrow[j];
							}
							if (oddrow[j] > biggest)
								biggest = oddrow[j];
						}
					}
				}
			}

			return biggest.ToString();
		}
	}
}
