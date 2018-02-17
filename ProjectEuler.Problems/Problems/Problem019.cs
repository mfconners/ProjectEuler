using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem019 : Problem
	{
		public override string CorrectAnswer { get { return "171"; } }

		protected override string CalculateSolution()
		{
			int count, year, month, day;
			int[] monthlength = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

			for (year = 1901, count = 0, day = 2; year <= 2000; year++)
			{
				for (month = 0; month < 12; month++)
				{
					if (day % 7 == 0)
						count++;
					day += monthlength[month];
					if (month == 1 && (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0)))
						day++;
					day %= 7;
				}
			}

			return count.ToString();
		}
	}
}
