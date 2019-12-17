using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem614 : Problem
	{
		// Slow: >1 hour
		public override string CorrectAnswer { get { return "130694090"; } }

		const int max_total = 10000000;
		//const int max_total = 1000;
		//const int max_total = 10000;
		const int half_max = max_total / 2;
		const long modulo_base = 1000000007;

		protected override string CalculateSolution()
		{
			List<long> P = new List<long>(Enumerable.Repeat<long>(0, max_total + 1));

			for (int summand = max_total; summand > half_max; --summand)
			{
				if (summand % 4 != 2)
				{
					P[summand] = 1;
				}
			}

			for (int summand = half_max; summand > 0; --summand)
			{
				if (summand % 4 != 2)
				{
					int n_min = 2 * summand;
					for (int n = max_total; n > n_min; --n)
					{
						P[n] += P[n - summand];
						if (P[n] >= 4611686018281801902)
						{
							P[n] -= 4611686018281801902;
						}
					}

					P[summand] = 1;
				}
			}

			long P_sum = 0;
			for (int n = 1; n <= max_total; ++n)
			{
				P_sum += P[n];
				if (P_sum >= 4611686018281801902)
				{
					P_sum -= 4611686018281801902;
				}
			}

			return (P_sum % modulo_base).ToString();
		}
	}
}
