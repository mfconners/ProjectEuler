using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem092 : Problem
	{
		public override string CorrectAnswer { get { return "8581146"; } }

		static protected int SumSquares(int num)
		{
			int sum = 0;
			int digit;
			while (num > 0)
			{
				digit = num % 10;
				num /= 10;
				sum += digit * digit;
			}

			return sum;
		}

		protected bool IsInLoop(int num, Dictionary<int, bool> loop)
		{
			if (loop.ContainsKey(num))
				return loop[num];

			int sumSquares = SumSquares(num);
			IsInLoop(sumSquares, loop);
			loop.Add(num, loop[sumSquares]);

			return loop[num];
		}

		protected override string CalculateSolution()
		{
			int count = 0;
			Dictionary<int, bool> LoopsWith4 = new Dictionary<int, bool>();
			LoopsWith4.Add(1, false);
			LoopsWith4.Add(4, true);

			for (int i = 1; i < 10000000; ++i)
			{
				if (LoopsWith4.ContainsKey(i))
				{
					if (LoopsWith4[i])
						++count;
				}
				else
				{
					if (IsInLoop(SumSquares(i), LoopsWith4))
						++count;
				}
			}

			return count.ToString();
		}
	}
}
