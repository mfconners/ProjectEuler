using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem116 : Problem
	{
		public override string CorrectAnswer { get { return "20492570929"; } }

		protected override string CalculateSolution()
		{
			ulong count = 0;

			List<ulong> anyRedBlackCombo = new List<ulong>();
			anyRedBlackCombo.Add(1);
			anyRedBlackCombo.Add(1);
			while (anyRedBlackCombo.Count < 49)
				anyRedBlackCombo.Add(anyRedBlackCombo[anyRedBlackCombo.Count - 1] +
														 anyRedBlackCombo[anyRedBlackCombo.Count - 2]);
			for (int firstRed = 0; firstRed < 49; ++firstRed)
				count += anyRedBlackCombo[50 - firstRed - 2];

			List<ulong> anyGreenBlackCombo = new List<ulong>();
			anyGreenBlackCombo.Add(1);
			anyGreenBlackCombo.Add(1);
			anyGreenBlackCombo.Add(1);
			while (anyGreenBlackCombo.Count < 48)
				anyGreenBlackCombo.Add(anyGreenBlackCombo[anyGreenBlackCombo.Count - 1] +
														 anyGreenBlackCombo[anyGreenBlackCombo.Count - 3]);
			for (int firstGreen = 0; firstGreen < 48; ++firstGreen)
				count += anyGreenBlackCombo[50 - firstGreen - 3];

			List<ulong> anyBlueBlackCombo = new List<ulong>();
			anyBlueBlackCombo.Add(1);
			anyBlueBlackCombo.Add(1);
			anyBlueBlackCombo.Add(1);
			anyBlueBlackCombo.Add(1);
			while (anyBlueBlackCombo.Count < 48)
				anyBlueBlackCombo.Add(anyBlueBlackCombo[anyBlueBlackCombo.Count - 1] +
														 anyBlueBlackCombo[anyBlueBlackCombo.Count - 4]);
			for (int firstBlue = 0; firstBlue < 47; ++firstBlue)
				count += anyBlueBlackCombo[50 - firstBlue - 4];

			return count.ToString();
		}
	}
}
