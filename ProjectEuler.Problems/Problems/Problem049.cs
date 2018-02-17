using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem049 : Problem
	{
		public override string CorrectAnswer { get { return "296962999629"; } }

		protected override string CalculateSolution()
		{
			List<int> digits = new List<int>();
			List<bool> fourFalse = new List<bool>();
			for (int i = 0; i < 4; ++i)
				fourFalse.Add(false);
			List<bool> taken = new List<bool>();
			List<int> permutations = new List<int>();

			for (int n = Primes.IndexOfPrimeAtLeast(1000); Primes.GetPrime(n) <= 9999; ++n)
			{
				permutations.Clear();
				permutations.Add(Primes.GetPrime(n));

				if (permutations[0] != 1487)
				{
					digits.Clear();
					for (int p = permutations[0]; p > 0; p /= 10)
						digits.Add(p % 10);

					for (int i = 1; i < 24; ++i)
					{
						int testPrime = 0;
						taken.InsertRange(0, fourFalse);
						for (int left = 4, setup = i; left > 0; setup /= left--)
						{
							for (int pos = 0, skipped = 0; pos - skipped <= setup % left; ++pos)
							{
								if (taken[pos])
									++skipped;
								if (pos - skipped == setup % left)
								{
									testPrime *= 10;
									testPrime += digits[pos];
									taken[pos] = true;
								}
							}
						}
						if (testPrime > permutations[0] && Primes.IsPrime(testPrime))
							permutations.Add(testPrime);
					}
				}

				for (int i = 1; i < permutations.Count; ++i)
				{
					for (int j = 1; j < permutations.Count; ++j)
					{
						if (permutations[i] < permutations[j])
							if (permutations[i] - permutations[0] == permutations[j] - permutations[i])
								return permutations[0].ToString() +
											 permutations[i].ToString() +
											 permutations[j].ToString();
					}
				}
			}
			return string.Empty;
		}
	}
}
