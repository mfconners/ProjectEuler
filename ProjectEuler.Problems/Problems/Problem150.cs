using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem150 : Problem
	{
		public override string CorrectAnswer { get { return "-271248680"; } }

		protected override string CalculateSolution()
		{
			List<List<Int64>> triangle = new List<List<Int64>>();
			for (int row = 0; row < 1000; ++row)
				triangle.Add(new List<Int64>());

			Int64 t = 0;
			for (int row = 0; row < 1000; ++row)
				for (int col = 0; col <= row; ++col)
				{
					t = (615949 * t + 797807) % (1 << 20);
					triangle[row].Add(t - (1 << 19));
				}

			Int64 minSum = 0;

			Dictionary<int, Dictionary<int, Int64>> lastRow = new Dictionary<int, Dictionary<int, Int64>>();
			for (int startCol = 0; startCol < 1000; ++startCol)
				lastRow.Add(startCol, new Dictionary<int, Int64>());

			for (int row = 999; row >= 0; --row)
			{
				Dictionary<int, Dictionary<int, Int64>> thisRow = new Dictionary<int, Dictionary<int, Int64>>();
				for (int startCol = 0; startCol <= row; ++startCol)
				{
					thisRow.Add(startCol, new Dictionary<int, Int64>());
					Int64 sum = 0;
					for (int endCol = startCol; endCol <= row; ++endCol)
					{
						sum += triangle[row][endCol];
						Int64 thisSum = sum;
						if (lastRow.ContainsKey(startCol) && lastRow[startCol].ContainsKey(endCol + 1) && sum + lastRow[startCol][endCol + 1] < 0)
							thisSum += lastRow[startCol][endCol + 1];

						if (thisSum < 0)
						{
							thisRow[startCol].Add(endCol, thisSum);
							if (startCol == endCol && thisSum < minSum)
								minSum = thisSum;
						}
					}
				}
				lastRow = thisRow;
			}

			return minSum.ToString();
		}
	}
}
