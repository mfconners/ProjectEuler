using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem387 : Problem
	{
		// This is a template problem...  Copy it and rename it to ProblemXXX, where XXX is the Project Euler problem number.
		//public override string CorrectAnswer { get { return "???"; } }

		protected override string CalculateSolution()
		{
			for (int i = 0;i<1000;++i)
			{
				i += i;
			}

			return "Yay!";
		}
	}
}
