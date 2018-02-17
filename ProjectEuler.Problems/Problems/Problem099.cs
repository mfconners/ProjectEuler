using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem099 : Problem
	{
		public override string CorrectAnswer { get { return "709"; } }

		private static readonly char[] separator = { ',' };

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			long bestRow = 0;
			long bestExp = 1, bestPower = 1;
			string [] base_exp_file = Properties.Resources.base_exp.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);

			int row = 0;
			foreach (string line in base_exp_file)
			{
				++row;
				string[] base_exp_data = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

				long newExp = Convert.ToInt64(base_exp_data[0]);
				long newPower = Convert.ToInt64(base_exp_data[1]);

				if (newPower * Math.Log(newExp) > bestPower * Math.Log(bestExp))
				{
					bestRow = row;
					bestExp = newExp;
					bestPower = newPower;
				}
			}

			return bestRow.ToString();
		}
	}
}
