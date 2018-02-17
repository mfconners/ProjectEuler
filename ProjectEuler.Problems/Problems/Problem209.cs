using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem209 : Problem
	{
		public override string CorrectAnswer { get { return "15964587728784"; } }

		protected override string CalculateSolution()
		{
			// BUG Problem 209 gets the wrong answer...
			List<int> neighbors = new List<int>();
			for (int i = 0; i < 64; ++i)
			{
				int neighbor = (i >> 4) & (i >> 3);
				neighbor = neighbor ^ (i >> 5);
				neighbor = neighbor & 0x1;
				neighbor = neighbor | (i << 1);
				neighbor = neighbor & 0x3F;
				neighbors.Add(neighbor);
			}

			List<int> neighborhoods = new List<int>();
			for (int i = 0; i < 64; ++i)
				for (int j = neighbors[i], n = 1;
						 j >= i;
						 j = neighbors[j], ++n)
					if (i == j)
					{
						neighborhoods.Add(n);
						break;
					}

			BigInteger count = 1;
			for (int i = 0; i < neighborhoods.Count; ++i)
			{
				BigInteger f1 = 0, f2 = 1, f3 = 1;
				for (int j = 1; j < neighborhoods[i]; ++j)
				{
					f1 = f2;
					f2 = f3;
					f3 = f1 + f2;
				}
				count *= (f1 + f3);
			}

			return count.ToString();
		}
	}
}
