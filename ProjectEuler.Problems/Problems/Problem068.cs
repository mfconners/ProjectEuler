using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem068 : Problem
	{
		public override string CorrectAnswer { get { return "6531031914842725"; } }

		protected override string CalculateSolution()
		{
			string solution = string.Empty;

			List<int> outer = new List<int>();
			outer.Add(6);
			outer.Add(7);
			outer.Add(8);
			outer.Add(9);
			outer.Add(10);
			List<int> inner = new List<int>();
			inner.Add(5);
			inner.Add(4);
			inner.Add(3);
			inner.Add(2);
			inner.Add(1);

			int rowSum = 0;
			for (int i = 0; i < 5; ++i)
				rowSum += outer[i] + 2 * inner[i];
			rowSum /= 5;

			List<int> outerTest = new List<int>(), innerTest = new List<int>();
			for (int i = 0; i < 5; ++i)
			{
				outerTest.Add(0);
				innerTest.Add(0);
			}

			for (int innerStart = 0; string.IsNullOrEmpty(solution); ++innerStart)
				for (int outerStep = 8; outerStep >= 4 && string.IsNullOrEmpty(solution); --outerStep)
					for (int innerStep = 1; innerStep < 5 && string.IsNullOrEmpty(solution); ++innerStep)
					{
						for (int i = 0; i < 5; ++i)
						{
							outerTest[i] = outer[(outerStep * i) % 5];
							innerTest[i] = inner[(innerStart + innerStep * i) % 5];
						}
						bool yay = true;
						for (int i = 0; yay && i < 5; ++i)
							if (outerTest[i] + innerTest[i] + innerTest[(i + 1) % 5] != rowSum)
								yay = false;

						if (yay)
						{
							for (int i = 0; i < 5; ++i)
							{
								solution += outerTest[i].ToString();
								solution += innerTest[i].ToString();
								solution += innerTest[(i + 1) % 5].ToString();
							}

						}
					}
			return solution;
		}
	}
}
