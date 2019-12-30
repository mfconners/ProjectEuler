using System.Collections.Generic;
using System.Linq;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem066 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		private void J(int D, long min_x, List<long> x, List<List<int>> dx, List<long> dy, List<long> diff)
		{
			while(x[D] < min_x || diff[D] > 1)
			{
				if (diff[D] == 1) return;
			}

			return;
		}

		protected override string CalculateSolution()
		{
			Queue<int> DQ = new Queue<int>();
			for (int i = 1, i_sq = 1, D = 2; D <= 1000; ++D)
			{
				while (D > i_sq) { i_sq = ++i * i; }
				if (D != i_sq)
				{
					DQ.Enqueue(D);
				}
			}

			List<long> x = new List<long>(1001);
			List<List<int>> dx = new List<List<int>>(1001);
			List<long> dy = new List<long>(1001);
			List<long> diff = new List<long>(1001);
			while (dy.Count <= 1000)
			{
				int D = x.Count;
				x.Add(0);
				if (D==0)
				{
					dx.Add(null);
				}
				else
				{
					dx.Add(new List<int>(Enumerable.Repeat<int>(1, D)));
					dx[D][D - 1] = 2;
					for (int idx = D-2; idx > 0; --idx)
					{
						if (Ratio.GreatestCommonDivisor(idx + 1, D) > 1)
						{
							dx[D][idx] = dx[D][idx + 1] + 1;
						}
					}
				}
				dy.Add(D);
				diff.Add(-D);
			}

			for (long min_x = 2500; DQ.Count > 1; min_x += 2500)
			{
				DQ.Enqueue(0);

				for (int D = DQ.Dequeue(); D != 0; D = DQ.Dequeue())
				{
					diff[D] += dx;

					if (diff[D] > 1)
					{
						dy[D] += 2 * D;
						diff[D] -= dy[D];
					}

					if (diff[D] != 1)
					{
						DQ.Enqueue(D);
					}
				}
			}

			return DQ.Dequeue().ToString();
		}
	}
}
