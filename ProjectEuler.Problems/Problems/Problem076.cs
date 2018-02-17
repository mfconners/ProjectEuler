using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem076 : Problem
	{
		public override string CorrectAnswer { get { return "190569291"; } }

		protected override string CalculateSolution()
		{
			List<List<int>> cache_count = new List<List<int>>();

			while (cache_count.Count <= 100)
				cache_count.Add(new List<int>());

			for (int sum = 1; sum <= 100; ++sum)
			{
				while (cache_count[sum].Count <= 99)
					cache_count[sum].Add(0);
				for (int max_adder = 1; max_adder <= 99; ++max_adder)
				{
					for (int max_adder_sum = max_adder; max_adder_sum <= sum; max_adder_sum += max_adder)
					{
						if (max_adder_sum == sum)
							cache_count[sum][max_adder] += 1;
						else
							cache_count[sum][max_adder] += cache_count[sum - max_adder_sum][max_adder - 1];
					}
					cache_count[sum][max_adder] += cache_count[sum][max_adder - 1];
				}
			}

			return cache_count[100][99].ToString();
		}
	}
}
