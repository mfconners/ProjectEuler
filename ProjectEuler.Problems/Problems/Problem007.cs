using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem007 : Problem
	{
		public override string CorrectAnswer { get { return "104743"; } }

		protected override string CalculateSolution()
		{
			return Primes.GetPrime(10000).ToString();
		}
	}
}
