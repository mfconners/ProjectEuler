using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem061 : Problem
	{
		public override string CorrectAnswer { get { return "28684"; } }

		protected override string CalculateSolution()
		{
			Dictionary<int, Dictionary<int, List<int>>> figurates = new Dictionary<int, Dictionary<int, List<int>>>();
			for (int s = 3; s <= 8; ++s)
				figurates.Add(s, new Dictionary<int, List<int>>());

			for (int n = 0; true; ++n)
			{
				bool ok = false;
				for (int s = 3; s <= 8; ++s)
				{
					int num = n * ((s - 2) * n + 4 - s) / 2;
					if (num <= 9999)
					{
						if (num >= 1000)
						{
							int start = num / 100, end = num % 100;
							if (!figurates[s].ContainsKey(start))
								figurates[s].Add(start, new List<int>());
							figurates[s][start].Add(end);
						}
						ok = true;
					}
				}
				if (!ok)
					break;
			}

			// Remaining unused figurates at each level
			List<HashSet<int>> remainingFigurates = new List<HashSet<int>>();
			while (remainingFigurates.Count < 6)
				remainingFigurates.Add(new HashSet<int>());
			for (int s = 3; s <= 8; ++s)
				remainingFigurates[0].Add(s);

			// Figurate selection at each level
			List<Queue<int>> untriedFigurates = new List<Queue<int>>();
			untriedFigurates.Add(new Queue<int>(remainingFigurates[0]));
			while (untriedFigurates.Count < 6)
				untriedFigurates.Add(new Queue<int>());

			List<int> curFigurate = new List<int>();
			while (curFigurate.Count < 6)
				curFigurate.Add(0);


			// Start selection at the first level.
			Queue<int> untriedStarts = new Queue<int>();
			int curStart = 0;

			// End select at each level (and start selection at the next level)...
			List<Queue<int>> untriedEnds = new List<Queue<int>>();
			while (untriedEnds.Count < 6)
				untriedEnds.Add(new Queue<int>());

			List<int> curEnd = new List<int>();
			while (curEnd.Count < 6)
				curEnd.Add(0);

			for (int l = 0; l < 6;)
			{
				if (l < 0)
					return SolutionUnknown;

				if (untriedEnds.Count > 0 && untriedEnds[l].Count > 0)
				{
					curEnd[l] = untriedEnds[l].Dequeue();
					++l;
					if (l < 6)
					{
						remainingFigurates[l] = new HashSet<int>(remainingFigurates[l - 1]);
						remainingFigurates[l].Remove(curFigurate[l - 1]);
						untriedFigurates[l] = new Queue<int>(remainingFigurates[l]);
					}
					else if (curStart != curEnd[5])
					{
						--l;
					}
				}
				else if (l == 0 && untriedStarts.Count > 0)
				{
					curStart = untriedStarts.Dequeue();
					untriedEnds[0] = new Queue<int>(figurates[curFigurate[0]][curStart]);
				}
				else if (untriedFigurates[l].Count() > 0)
				{
					curFigurate[l] = untriedFigurates[l].Dequeue();

					if (l == 0)
					{
						untriedStarts = new Queue<int>(figurates[curFigurate[0]].Keys);
					}
					else if (figurates[curFigurate[l]].ContainsKey(curEnd[l - 1]))
					{
						untriedEnds[l] = new Queue<int>(figurates[curFigurate[l]][curEnd[l - 1]]);
					}
				}
				else
				{
					--l;
				}
			}

			int sum = 0;
			for (int l = 0; l < 6; ++l)
			{
				sum += curEnd[l] * 101;
			}
			return sum.ToString();
		}
	}
}
