using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem004 : Problem
	{
		public override string CorrectAnswer { get { return "906609"; } }

		protected override string CalculateSolution()
		{
			SortedList<ulong, ulong> queue = new SortedList<ulong, ulong>();
			KeyValuePair<ulong, ulong> nextTest;
			queue.Add(999 * 999, 999);

			ulong nextThreeDigitNumber = 998;
			ulong nextSquaredThreeDigitNumber = nextThreeDigitNumber * nextThreeDigitNumber;

			while (!BigIntegerOps.IsPalindrome((nextTest = queue.Last()).Key))
			{
				ulong tryMultiple = nextTest.Key - nextTest.Value;
				while (queue.ContainsKey(tryMultiple))
					tryMultiple -= nextTest.Value;

				queue.Add(tryMultiple, nextTest.Value);
				queue.Remove(nextTest.Key);

				if (nextSquaredThreeDigitNumber > queue.Last().Key)
				{
					queue.Add(nextSquaredThreeDigitNumber, nextThreeDigitNumber);
					nextThreeDigitNumber--;
					nextSquaredThreeDigitNumber = nextThreeDigitNumber * nextThreeDigitNumber;
				}
			}

			return nextTest.Key.ToString();
		}
	}
}
