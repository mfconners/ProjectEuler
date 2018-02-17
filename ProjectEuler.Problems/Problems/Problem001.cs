using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem001 : Problem
	{
		public override string CorrectAnswer { get { return "233168"; } }

		protected override string CalculateSolution()
		{
			ulong i;
			ulong sum = 0;

			for (i = 1; i < 1000; i++)
				if (i % 3 == 0 || i % 5 == 0)
					sum += i;

			return sum.ToString();
		}
	}
}
