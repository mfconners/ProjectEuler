using System;
using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem060 : Problem
	{
		public override string CorrectAnswer { get { return "26033"; } }

		private const int search_size = 5;

		public int SumPrimePairs(List<int> prime_indexes, HashSet<int> overlaps, Dictionary<int, HashSet<int>> prime_pairs, int layer)
		{
			if (layer >= search_size)
				return Primes.GetPrime(prime_indexes[layer - 1]);

			if (overlaps == null)
				return 0;

			int min_sum = 0;

			if (overlaps.Count >= search_size - layer)
			{
				foreach (int overlap in overlaps)
				{
					HashSet<int> new_overlaps = null;
					prime_indexes[layer] = overlap;

					if (prime_pairs.ContainsKey(overlap) && prime_pairs[overlap] != null && prime_pairs[overlap].Count + 1 >= search_size - layer)
					{
						new_overlaps = new HashSet<int>(prime_pairs[overlap]);
						new_overlaps.IntersectWith(overlaps);
					}

					int sum = SumPrimePairs(prime_indexes, new_overlaps, prime_pairs, layer + 1);

					if (sum > 0)
						if (min_sum == 0 || sum < min_sum)
							min_sum = sum;
				}

				if (min_sum > 0)
					min_sum += Primes.GetPrime(prime_indexes[layer - 1]);
			}

			return min_sum;
		}

		protected override string CalculateSolution()
		{
			List<int> prime_indexes = new List<int>(search_size);
			Dictionary<int, HashSet<int>> prime_pairs = new Dictionary<int, HashSet<int>>();
			while (prime_indexes.Count < search_size)
			{
				prime_indexes.Add(0);
			}

			int sum = 0;
			while (sum == 0)
			{
				++prime_indexes[0];
				string bigger = Primes.GetPrime(prime_indexes[0]).ToString();

				for (int j = 1; j < prime_indexes[0]; ++j)
				{
					string littler = Primes.GetPrime(j).ToString();
					if (Primes.IsPrime(Convert.ToInt32(littler + bigger), false) && Primes.IsPrime(Convert.ToInt32(bigger + littler), false))
					{
						if (!prime_pairs.ContainsKey(prime_indexes[0]))
						{
							prime_pairs.Add(prime_indexes[0], new HashSet<int>());
						}

						prime_pairs[prime_indexes[0]].Add(j);
					}
				}

				if (prime_pairs.ContainsKey(prime_indexes[0]))
				{
					sum = SumPrimePairs(prime_indexes, prime_pairs[prime_indexes[0]], prime_pairs, 1);
				}
			}

			return sum.ToString();
		}
	}
}
