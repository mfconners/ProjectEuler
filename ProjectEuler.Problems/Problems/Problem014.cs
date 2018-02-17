using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem014 : Problem
	{
		public override string CorrectAnswer { get { return "837799"; } }

		private static ulong oneMillion = 1000000;
		private Dictionary<ulong, ulong> Counts;

		private ulong GetCount(ulong start)
		{
			if (!Counts.ContainsKey(start))
			{
				ulong next;
				if ((start & 0x1) == 0)
					next = start / 2;
				else
					next = 3 * start + 1;

				Counts.Add(start, GetCount(next) + 1);
			}

			return Counts[start];
		}

		protected override string CalculateSolution()
		{
			Counts = new Dictionary<ulong, ulong>();
			Counts.Add(1, 0);

			ulong maxCount = 0, maxStart = 1;
			for (ulong start = 2; start < oneMillion; start++)
			{
				ulong count = GetCount(start);

				if (count > maxCount)
				{
					maxCount = count;
					maxStart = start;
				}
			}

			Counts = null;

			return maxStart.ToString();
		}
	}
}
