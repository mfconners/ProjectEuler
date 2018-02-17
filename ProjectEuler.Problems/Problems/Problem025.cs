using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem025 : Problem
	{
		public override string CorrectAnswer { get { return "4782"; } }

		private static readonly double phi = (Math.Sqrt(5) + 1) / 2;

		protected override string CalculateSolution()
		{
			double exp = (999 + Math.Log10(Math.Sqrt(5)) / 2) / Math.Log10(phi);

			return Math.Ceiling(exp).ToString();
		}
	}
}
