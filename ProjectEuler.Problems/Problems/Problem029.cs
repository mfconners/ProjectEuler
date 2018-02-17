using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem029 : Problem
	{
		public override string CorrectAnswer { get { return "9183"; } }

		protected override string CalculateSolution()
		{
			SortedSet<double> terms = new SortedSet<double>();

			for (int a = 2; a <= 100; ++a)
			{
				for (int b = 2; b <= 100; ++b)
				{
					double term = Math.Pow(a, b);
					if (!terms.Contains(term))
						terms.Add(term);
				}
			}


			return terms.Count.ToString();
		}
	}
}
