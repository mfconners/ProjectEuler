using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem013 : Problem
	{
		public override string CorrectAnswer { get { return "5537376230"; } }

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			BigInteger bignum;
			BigInteger biggernum = 0;

			string[] lines = Properties.Resources.p013.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);

			foreach (string nextline in lines)
			{
				bignum = BigInteger.Parse(nextline);
				biggernum += bignum;
			}

			return biggernum.ToString().Remove(10);
		}
	}
}
