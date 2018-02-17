using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem022 : Problem
	{
		public override string CorrectAnswer { get { return "871198282"; } }

		private static char[] separators = { '\"', ',', '\r', '\n' };

		protected override string CalculateSolution()
		{
			long score = 0;

			List<string> names_file = new List<string>(Properties.Resources.names.Split(separators, StringSplitOptions.RemoveEmptyEntries));

			names_file.Sort();
			for (int i = 0; i < names_file.Count; ++i)
			{
				string name = names_file[i];
				int namescore = 0;

				for (int j = 0; j < names_file[i].Length; ++j)
				{
					namescore += name[j] - 'A' + 1;
				}

				score += namescore * (i + 1);
			}

			return score.ToString();
		}
	}
}
