using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem063 : Problem
	{
		public override string CorrectAnswer { get { return "49"; } }

		protected override string CalculateSolution()
		{
			int count = 0;
			for (BigInteger i = 1; i < 10; ++i)
				for (BigInteger power = i, mod = 1; power >= mod; power *= i, mod *= 10)
					++count;

			return count.ToString();
		}
	}
}
