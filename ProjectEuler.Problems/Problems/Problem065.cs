using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem065 : Problem
	{
		public override string CorrectAnswer { get { return "272"; } }

		protected override string CalculateSolution()
		{
			List<int> top = new List<int>();
			top.Add(1);
			List<int> bottom = new List<int>(top);

			int numConvergents = 100;

			for (int c = numConvergents; c > 0; --c)
			{
				int multiplier = 1;
				if (c % 3 == 0)
				{
					multiplier = 2 * c / 3;
				}
				else if (c == 1)
				{
					multiplier = 2;
				}

				List<int> newTop = new List<int>();

				if (c == numConvergents)
				{
					for (int accumulator = multiplier; accumulator != 0; accumulator /= 10)
						newTop.Add(accumulator % 10);
				}
				else
				{
					for (int i = 0, accumulator = 0; i < top.Count || accumulator != 0 || i < bottom.Count; ++i)
					{
						if (i < top.Count)
							accumulator += multiplier * top[i];
						if (i < bottom.Count)
							accumulator += bottom[i];
						newTop.Add(accumulator % 10);
						accumulator /= 10;
					}
				}

				bottom = top;
				top = newTop;
			}

			int sum = 0;
			foreach (int digit in top)
				sum += digit;


			return sum.ToString();
		}
	}
}
