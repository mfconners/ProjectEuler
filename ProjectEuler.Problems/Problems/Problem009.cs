using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem009 : Problem
	{
		public override string CorrectAnswer { get { return "31875000"; } }

		protected override string CalculateSolution()
		{
			ulong a = 1, b = 1, c = 998;

			for (a = 1; a < c; a++)
			{
				for (b = 1; b < (c = 1000 - a - b) && (a * a + b * b != c * c); b++)
					;
				if (a * a + b * b == c * c)
					break;
			}

			return (a * b * c).ToString();
		}
	}
}
