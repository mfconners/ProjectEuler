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
		public override string CorrectAnswer { get { return "130694090"; } }

		const int max_total = 10000000;
		//const int max_total = 1000;
		const int half_max = max_total / 2;
		const long modulo_base = 1000000007;

		//private string ImprovedSolution(StreamReader file)
		//{
		//	List<long> P = new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5));
		//	List<long> Q = new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5));
		//	List<long> R = new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5));
		//	List<long> RR = new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5));

		//	List<List<long>> even_data = new List<List<long>>();
		//	even_data.Add(new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5)));
		//	even_data.Add(new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5)));

		//	List<List<long>> odd_data = new List<List<long>>();
		//	odd_data.Add(new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5)));
		//	odd_data.Add(new List<long>(Enumerable.Repeat((long)0, 2 * max_total + 5)));

		//	List<long> U = new List<long>(Enumerable.Repeat((long)0, max_total + 5));
		//	List<long> SU = new List<long>(Enumerable.Repeat((long)0, max_total + 5));
		//	List<long> V = new List<long>(Enumerable.Repeat((long)0, max_total + 5));

		//	R[0] = 1;
		//	RR[0] = 1;

		//	{
		//		List<long> ef = even_data[0];
		//		List<long> et = even_data[0];
		//		List<long> of = odd_data[0];
		//		List<long> ot = odd_data[0];
		//		even_data[0][0] = 1;

		//		for (int k = 1; k*k <= 2 * max_total; ++k)
		//		{
		//			fill(et, et + max_total + 1, 0);
		//			fill(ot, ot + max_total + 1, 0);
		//			int k2 = k * k;
		//			for (int i = k2; i <= 2 * max_total; ++i)
		//			{
		//				et[i] = ot[i - k];
		//				ot[i] = add_mod(et[i - k], ef[i - k], modulo_base);
		//				R[i] = add_mod(R[i], ot[i], modulo_base);
		//				RR[i] = add_mod(RR[i], et[i], modulo_base);
		//			}

		//			List<long> temp = ef;
		//			ef = et;
		//			et = temp;
		//			temp = of;
		//			of = ot;
		//			ot = temp;
		//		}
		//	}
		//	for (int i = 1; i <= max_total; ++i)
		//	{
		//		Q[i] = RR[2 * i];
		//	}
		//	Q[0] = 1;
		//	for (int i = 0; i <= max_total; i += 4)
		//	{
		//		U[i] = Q[i / 4];
		//	}
		//	SU[0] = U[0];
		//	for (int i = 1; i <= max_total; ++i)
		//	{
		//		SU[i] = add_mod(U[i], SU[i - 1], modulo_base);
		//	}
		//	for (int i = 0; i <= max_total; ++i)
		//	{
		//		V[i] = R[i];
		//	}

		//	long P_sum = modulo_base - 1;
		//	for (int i = 0; i <= max_total; ++i)
		//	{
		//		P_sum = add_mod(P_sum, (long)V[i] * SU[max_total - i] % modulo_base, modulo_base);
		//	}
		//	return 0;

		//}

		protected override string CalculateSolution()
		{
			List<long> P = new List<long>(Enumerable.Repeat<long>(0, max_total + 1));

			for (int summand = max_total; summand > half_max; --summand)
			{
				if (summand % 2 != 0 || summand % 4 == 0)
				{
					P[summand] = 1;
				}
			}

			for (int summand = half_max; summand > 0; --summand)
			{
				if (summand % 2 != 0 || summand % 4 == 0)
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
				P_sum %= modulo_base;
			}

			return P_sum.ToString();
		}
	}
}
