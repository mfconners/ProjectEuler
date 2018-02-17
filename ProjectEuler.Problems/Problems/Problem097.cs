using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem097 : Problem
	{
		static readonly BigInteger tenBillion = 10000000000;

		public override string CorrectAnswer { get { return "8739992577"; } }

		protected override string CalculateSolution()
		{
			BigInteger lastTenDigits;
			BigInteger exponentOfTwo = 7830457;
			ulong twoToTheTwentyEight = 0x1 << 28;

			lastTenDigits = 28433;
			while (exponentOfTwo >= 28)
			{
				lastTenDigits *= twoToTheTwentyEight;
				lastTenDigits %= tenBillion;
				exponentOfTwo -= 28;
			}
			while (exponentOfTwo > 0)
			{
				lastTenDigits *= 2;
				exponentOfTwo--;
			}

			return (lastTenDigits % tenBillion + 1).ToString();
		}
	}
}
