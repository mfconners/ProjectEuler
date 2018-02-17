using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem071 : Problem
	{
		public override string CorrectAnswer { get { return "428570"; } }

		protected override string CalculateSolution()
		{
			int n = 1, d = 3;

			for (int test_d = 4; test_d <= 1000000; ++test_d)
			{
				if (test_d % 7 == 0)
					continue;

				int test_n = 3 * test_d / 7;

				if (test_n * d > n * test_d)
				{
					n = test_n;
					d = test_d;
				}
			}

			return n.ToString();
		}
	}
}
