using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem618 : Problem
	{
		public override string CorrectAnswer { get { return "634212216"; } }

		protected override string CalculateSolution()
		{
			Dictionary<int, int> fibonacci = new Dictionary<int, int>();
			fibonacci.Add(1, 1);
			fibonacci.Add(2, 1);
			while (fibonacci.Count < 24)
			{
				int next = fibonacci.Count + 1;
				fibonacci.Add(next, fibonacci[next - 2] + fibonacci[next - 1]);
			}

			Dictionary<int, long> S_mod_billion = new Dictionary<int, long>();
			S_mod_billion.Add(0, 1);
			for (int s = 1; s <= fibonacci[24]; ++s)
			{
				S_mod_billion.Add(s, 0);
			}

			for (int p = 0, prime = Primes.GetPrime(0); prime <= fibonacci[24]; prime = Primes.GetPrime(++p))
			{
				for (int s = 0; s + prime <= fibonacci[24]; ++s)
				{
					if (S_mod_billion[s] > 0)
					{
						S_mod_billion[s + prime] += prime * S_mod_billion[s];
						S_mod_billion[s + prime] %= 1000000000;
					}
				}
			}

			long S_sum = 0;
			for (int f = 2; f <= 24; ++f)
			{
				S_sum += S_mod_billion[fibonacci[f]];
				S_sum %= 1000000000;
			}

			return S_sum.ToString();
		}
	}
}
