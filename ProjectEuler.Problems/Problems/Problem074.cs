using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
	class Problem074 : Problem
	{
		public override string CorrectAnswer { get { return "402"; } }

		protected int ChainCount(
			int n,
			List<int> factorial,
			Dictionary<int, int> chain_count
			)
		{
			if (chain_count.ContainsKey(n))
				return chain_count[n];

			int fact_sum = 0;
			for (int remainder = n; remainder > 0; remainder /= 10)
			{
				fact_sum += factorial[remainder % 10];
			}

			if (fact_sum == n)
			{
				chain_count.Add(n, 1);
				return 1;
			}
			int count = ChainCount(fact_sum, factorial, chain_count) + 1;
			chain_count.Add(n, count);
			return count;
		}

		protected override string CalculateSolution()
		{
			int winner_count = 0;

			List<int> factorial = new List<int>(10);
			for (int f = 0, fact = 1; f < 10; fact *= (++f))
			{
				factorial.Add(fact);
			}

			Dictionary<int, int> chain_count = new Dictionary<int, int>();
			// 169 -> 363601 -> 1454 -> 169
			chain_count.Add(169, 3);
			chain_count.Add(363601, 3);
			chain_count.Add(1454, 3);
			// 871 -> 45361 -> 871
			chain_count.Add(871, 2);
			chain_count.Add(45361, 2);
			// 872 -> 45362 -> 872
			chain_count.Add(872, 2);
			chain_count.Add(45362, 2);

			for (int n = 999999; n > 1; --n)
			{
				if (!chain_count.ContainsKey(n))
				{
					if (ChainCount(n, factorial, chain_count) == 60)
					{
						++winner_count;
					}
				}
			}

			return winner_count.ToString();
		}
	}
}
