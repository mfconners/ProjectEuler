using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem006 : Problem
	{
		public override string CorrectAnswer { get { return "25164150"; } }

		const int max = 100;

		protected override string CalculateSolution()
		{
			BigInteger sumSquares = 0, squareSums = 0;
			int i;

			for (i = 0; i <= max; i++)
			{
				sumSquares += i * i;
				squareSums += i;
			}
			squareSums = squareSums * squareSums;

			return (squareSums - sumSquares).ToString();
		}
	}
}
