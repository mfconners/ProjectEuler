using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem015 : Problem
	{
		public override string CorrectAnswer { get { return "137846528820"; } }

		protected override string CalculateSolution()
		{
			List<List<ulong>> grid = new List<List<ulong>>(21);

			for (int i = 0; i <= 20; i++)
			{
				grid.Add(new List<ulong>(21));
				for (int j = 0; j <= 20; j++)
					grid[i].Add(0);
			}

			grid[0][0] = 1;
			for (int i = 0; i < 21; i++)
			{
				for (int j = 0; j < 21; j++)
				{
					if (i > 0 && j > 0)
					{
						grid[i][j] = grid[i - 1][j] + grid[i][j - 1];
					}
					else
					{
						grid[i][j] = 1;
					}
				}
			}

			return grid[20][20].ToString();
		}
	}
}
