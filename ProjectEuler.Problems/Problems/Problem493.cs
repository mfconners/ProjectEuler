using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem493 : Problem
	{
		public override string CorrectAnswer { get { return "6.818741802"; } }

		protected override string CalculateSolution()
		{
			List<double> current = new List<double>(8);
			List<double> next = new List<double>(8);

			while (current.Count < 8)
			{
				current.Add(0.0);
				next.Add(0.0);
			}
			current[1] = 1.0;

			for (int balls = 1; balls < 20; ++balls)
			{
				for (int distinct = 1; distinct <= 7; ++distinct)
				{
					next[distinct] =
						current[distinct] * (10 * distinct - balls) / (70 - balls)
						+ current[distinct - 1] * (70 - 10 * (distinct - 1)) / (70 - balls);
				}

				List<double> tmp = current;
				current = next;
				next = tmp;
				tmp = null;
			}

			double expected = 0.0;
			for (int distinct = 1; distinct <= 7; ++distinct)
			{
				expected += distinct * current[distinct];
			}

			return expected.ToString().Substring(0, 11);
		}
	}
}
