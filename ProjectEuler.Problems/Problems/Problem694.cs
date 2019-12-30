using ProjectEuler.MathExtensions;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem694 : Problem
	{
		public override string CorrectAnswer { get { return "1339784153569958487"; } }

		private long S(long n, int min_p = 0, Dictionary<(long n, int min_p), long> S_cache = null)
		{
			long sum = n;
			if (S_cache == null) S_cache = new Dictionary<(long n, int min_p), long>();

			long prime = 0, cube_full = 0;
			for (int p = min_p; (cube_full = (prime = Primes.GetPrime(p)) * prime * prime) <= n; ++p)
			{
				for (long cube_full_max = n / prime; cube_full <= n; cube_full = (cube_full <= cube_full_max ? cube_full * prime : long.MaxValue))
				{
					sum += S(n / cube_full, p + 1);
				}
			}

			return sum;
		}

		protected override string CalculateSolution()
		{
			//return S(16).ToString(); // 19
			//return S(100).ToString(); // 126
			//return S(10000).ToString(); // 13344
			return S((long)1000 * 1000 * 1000 * 1000 * 1000 * 1000).ToString();
		}
	}
}
