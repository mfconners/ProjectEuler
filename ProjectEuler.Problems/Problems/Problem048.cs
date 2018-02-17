using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem048 : Problem
	{
		public override string CorrectAnswer { get { return "9110846700"; } }

		static private long mask = 10000000000;

		protected override string CalculateSolution()
		{
			long sum = 0;

			for (int i = 1; i <= 1000; ++i)
			{
				if (i % 10 > 0)
				{
					long power = 1;
					for (int j = 0; j < i; ++j)
					{
						power *= i;
						power %= mask;
					}
					sum += power;
					sum %= mask;
				}
			}

			return sum.ToString();
		}
	}
}
