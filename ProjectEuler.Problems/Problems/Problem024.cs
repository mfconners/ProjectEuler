using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem024 : Problem
	{
		public override string CorrectAnswer { get { return "2783915460"; } }

		protected override string CalculateSolution()
		{
			string remaining = "0123456789";
			string millionth = string.Empty;
			int n = 999999;
			int size = 9 * 8 * 7 * 6 * 5 * 4 * 3 * 2 * 1;

			while (n > 0)
			{
				millionth += remaining[n / size];
				remaining = remaining.Remove(n / size, 1);
				n %= size;
				size /= remaining.Count();
			}

			millionth += remaining;

			return millionth;
		}
	}
}
