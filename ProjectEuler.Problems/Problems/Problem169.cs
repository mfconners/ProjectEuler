using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem169 : Problem
	{
		public override string CorrectAnswer { get { return "178653872807"; } }

		protected override string CalculateSolution()
		{
			Int64 noCarry = 1, carry = 0;
			for (int i = 0; i < 25; ++i)
			{
				carry += noCarry;
			}

			Int64 remainingBits = 1;
			for (int i = 0; i < 25; ++i)
			{
				checked
				{
					remainingBits *= 5;
				}
			}

			while (remainingBits > 0)
			{
				if (remainingBits % 2 == 0)
					carry += noCarry;
				else
					noCarry += carry;

				remainingBits /= 2;
			}

			return noCarry.ToString();
		}
	}
}
