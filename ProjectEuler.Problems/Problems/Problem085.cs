using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem085 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		protected override string CalculateSolution()
		{
			for (int i = 0; i < 1000; ++i)
			{
				i += i;
			}

			return "Yay!";
		}
	}
}
