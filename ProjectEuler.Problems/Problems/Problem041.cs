using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem041 : Problem
	{
		public override string CorrectAnswer { get { return "7652413"; } }

		protected override string CalculateSolution()
		{
			// Any 9- or 8-digit pandigital number is divisible by 3, so non-otherbits.
			// Start with the 7-digit primes...
			for (int primeTest = 7654321;
					true;
					primeTest -= 2)
			{
				if (Primes.IsPrime(primeTest, false))
					if (BigIntegerOps.IsPanDigital(primeTest))
						return primeTest.ToString();
			}
			/*
			for (int primeIndex = Primes.IndexOfPrimeAtMost(7654321);
					primeIndex > 0;
					--primeIndex)
			{
				if (BigIntegerOps.IsPanDigital(Primes.GetPrime(primeIndex)))
					return Primes.GetPrime(primeIndex).ToString();
			}
			*/
		}
	}
}
