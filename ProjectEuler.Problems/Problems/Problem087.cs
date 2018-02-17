using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem087 : Problem
	{
		public override string CorrectAnswer { get { return "1097343"; } }

		private const int sumMax = 50000000;

		protected override string CalculateSolution()
		{
			List<int> primeSquares = new List<int>();
			List<int> primeCubes = new List<int>();
			List<int> primeQuads = new List<int>();

			for (int p = 0, primeSquare = Primes.GetPrime(0) * Primes.GetPrime(0); primeSquare < sumMax; ++p, primeSquare = Primes.GetPrime(p) * Primes.GetPrime(p))
				primeSquares.Add(primeSquare);
			for (int p = 0, primeCube = Primes.GetPrime(0) * primeSquares[0]; primeCube < sumMax; ++p, primeCube = Primes.GetPrime(p) * primeSquares[p])
				primeCubes.Add(primeCube);
			for (int p = 0, primeQuad = primeSquares[0] * primeSquares[0]; primeQuad < sumMax; ++p, primeQuad = primeSquares[p] * primeSquares[p])
				primeQuads.Add(primeQuad);

			HashSet<int> sums = new HashSet<int>();

			for (int i = 0; i < primeSquares.Count; ++i)
				for (int j = 0, firstSum; j < primeCubes.Count && (firstSum = primeSquares[i] + primeCubes[j]) < sumMax; ++j)
					for (int k = 0, sumTotal; k < primeQuads.Count && (sumTotal = firstSum + primeQuads[k]) < sumMax; ++k)
						sums.Add(sumTotal);

			return sums.Count.ToString();
		}
	}
}
