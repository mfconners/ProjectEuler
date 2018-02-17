using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem011 : Problem
	{
		public override string CorrectAnswer { get { return "70600674"; } }

		private static readonly char[] separator = { ' ' };

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			List<List<ulong>> grid = new List<List<ulong>>(20);
			ulong cur, max = 0;
			int i, j;

			for (i = 0; i < 20; i++)
			{
				for (j = 0; j < 20; j++)
				{
				}
			}

			string[] lines = Properties.Resources.p011.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);
			for (i = 0; i < 20; i++)
			{
				grid.Add(new List<ulong>(20));
				string[] numbers = lines[i].Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
				for (j = 0; j < 20; j++)
				{
					grid[i].Add(ulong.Parse(numbers[j]));
				}
			}

			for (i = 0; i < 20; i++)
			{
				for (j = 0; j < 20; j++)
				{
					if (i < 17)
					{
						if (j < 17)
						{
							cur = grid[i][j];
							cur *= grid[i + 1][j + 1];
							cur *= grid[i + 2][j + 2];
							cur *= grid[i + 3][j + 3];
							if (cur > max)
								max = cur;
						}
						cur = grid[i][j];
						cur *= grid[i + 1][j];
						cur *= grid[i + 2][j];
						cur *= grid[i + 3][j];
						if (cur > max)
							max = cur;
						if (j > 3)
						{
							cur = grid[i][j];
							cur *= grid[i + 1][j - 1];
							cur *= grid[i + 2][j - 2];
							cur *= grid[i + 3][j - 3];
							if (cur > max)
								max = cur;
						}
					}
					if (j < 17)
					{
						cur = grid[i][j];
						cur *= grid[i][j + 1];
						cur *= grid[i][j + 2];
						cur *= grid[i][j + 3];
						if (cur > max)
							max = cur;
					}
				}
			}

			return max.ToString();
		}
	}
}
