using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem031 : Problem
	{
		public override string CorrectAnswer { get { return "73682"; } }

		private static int[] COINS = { 200, 100, 50, 20, 10, 5, 2 };

		protected override string CalculateSolution()
		{
			List<int> coinSizes = new List<int>(COINS);
			List<int> penceRemainder = new List<int>(coinSizes.Count - 1);
			for (int i = 0; i < coinSizes.Count - 1; i++)
			{
				penceRemainder.Add(200);
			}
			int count = 0;

			bool reset = true;
			for (int i = penceRemainder.Count; i >= 0; i += (reset ? +1 : -1))
			{
				if (i == penceRemainder.Count)
				{
					count += penceRemainder[i - 1] / coinSizes[i] + 1;
					reset = false;
				}
				else if (reset)
				{
					penceRemainder[i] = penceRemainder[i - 1];
				}
				else if (penceRemainder[i] >= coinSizes[i])
				{
					reset = true;
					penceRemainder[i] -= coinSizes[i];
				}
			}

			return count.ToString();
		}
	}
}
