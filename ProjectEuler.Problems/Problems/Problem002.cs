using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem002 : Problem
	{
		public override string CorrectAnswer { get { return "4613732"; } }

		protected override string CalculateSolution()
		{
			int i;
			long sum = 0;

			for (i = 2; Fibonacci.GetFibonacci(i) <= 4000000; i += 3)
			{
				sum += Fibonacci.GetFibonacci(i);
			}

			return sum.ToString();
		}
	}
}
