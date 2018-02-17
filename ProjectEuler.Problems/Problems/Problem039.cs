using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem039 : Problem
	{
		public override string CorrectAnswer { get { return "840"; } }

		protected override string CalculateSolution()
		{
			int maxCount = 0;
			int maxCountPerimeter = 0;
			for (int p = 3; p <= 1000; ++p)
			{
				int count = 0;
				for (int a = 1, c = p / 2; a < p - c - a + 2; ++a)
				{
					int b = p - c - a;
					int diff = c * c - b * b - a * a;
					while (diff > 0 && diff > c + b)
					{
						--c;
						++b;
						diff = c * c - b * b - a * a;
					}
					while (diff < 0 && diff + c + b < 0)
					{
						++c;
						--b;
						diff = c * c - b * b - a * a;
					}
					if (diff == 0 && a < b)
					{
						++count;
					}
				}

				if (count > maxCount)
				{
					maxCount = count;
					maxCountPerimeter = p;
				}
			}

			return maxCountPerimeter.ToString();
		}
	}
}
