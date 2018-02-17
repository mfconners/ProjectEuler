using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem117 : Problem
	{
		public override string CorrectAnswer { get { return "100808458960497"; } }

		protected override string CalculateSolution()
		{
			ulong count = 1;

			List<ulong> anyRGBBlackCombo = new List<ulong>();
			anyRGBBlackCombo.Add(1);
			anyRGBBlackCombo.Add(1);
			anyRGBBlackCombo.Add(anyRGBBlackCombo[0] +
													 anyRGBBlackCombo[1]);
			anyRGBBlackCombo.Add(anyRGBBlackCombo[0] +
													 anyRGBBlackCombo[1] +
													 anyRGBBlackCombo[2]);

			while (anyRGBBlackCombo.Count < 49)
				anyRGBBlackCombo.Add(anyRGBBlackCombo[anyRGBBlackCombo.Count - 1] +
														 anyRGBBlackCombo[anyRGBBlackCombo.Count - 2] +
														 anyRGBBlackCombo[anyRGBBlackCombo.Count - 3] +
														 anyRGBBlackCombo[anyRGBBlackCombo.Count - 4]);

			for (int firstRed = 0; firstRed < 49; ++firstRed)
				count += anyRGBBlackCombo[50 - firstRed - 2];
			for (int firstGreen = 0; firstGreen < 48; ++firstGreen)
				count += anyRGBBlackCombo[50 - firstGreen - 3];
			for (int firstBlue = 0; firstBlue < 47; ++firstBlue)
				count += anyRGBBlackCombo[50 - firstBlue - 4];

			return count.ToString();
		}
	}
}
