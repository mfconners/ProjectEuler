using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem028 : Problem
	{
		public override string CorrectAnswer { get { return "669171001"; } }

		protected override string CalculateSolution()
		{
			int sum = 1001;
			sum = (sum + 1) / 2;
			sum = 16 * sum * sum * sum - 18 * sum * sum + 14 * sum - 9;
			sum /= 3;

			return sum.ToString();
		}
	}
}
