using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem075 : Problem
	{
		public override string CorrectAnswer { get { return "161667"; } }

		static private void generate_primitives(Dictionary<long, int> triangle_mult_count, long m, long n)
		{
			long L = 2 * m * (m + n);
			if (L > 1500000)
				return;

			for (long L_mult = L; L_mult <= 1500000; L_mult += L)
			{
				if (!triangle_mult_count.ContainsKey(L_mult))
					triangle_mult_count.Add(L_mult, 1);
				else
					++triangle_mult_count[L_mult];
			}

			generate_primitives(triangle_mult_count, 2 * m - n, m);
			generate_primitives(triangle_mult_count, 2 * m + n, m);
			generate_primitives(triangle_mult_count, m + 2 * n, n);
		}

		protected override string CalculateSolution()
		{
			long singles_count = 0;

			Dictionary<long, int> triangle_mult_count = new Dictionary<long, int>();

			generate_primitives(triangle_mult_count, 2, 1);

			foreach (var L in triangle_mult_count.Keys)
			{
				if (triangle_mult_count[L] == 1)
				{
					++singles_count;
				}
			}
			//Dictionary<long, long> squares = new Dictionary<long, long>();
			//for (long i = 2; i <= 750000; ++i)
			//	squares.Add(i, i * i);

			//Dictionary<long, long> triangle_mult_count = new Dictionary<long, long>();

			//for (long L = 10; L <= 1500000; L += 2)
			//{
			//	if (!triangle_mult_count.ContainsKey(L) || triangle_mult_count[L] <= 1)
			//	{
			//		long int_triangle_count = 0;
			//		for (long i = 2, j = (L - 3) / 2; i < j && int_triangle_count < 2; ++i)
			//		{
			//			while (squares[i] + squares[j] > squares[L - i - j])
			//				--j;

			//			if (squares[i] + squares[j] == squares[L - i - j] && i < j)
			//				++int_triangle_count;
			//		}

			//		if (int_triangle_count == 1)
			//		{
			//			++singles_count;

			//			if (!triangle_mult_count.ContainsKey(L))
			//			{
			//				for (long i = 2 * L; i <= 1500000; i += L)
			//				{
			//					if (!triangle_mult_count.ContainsKey(i))
			//					{
			//						triangle_mult_count.Add(i, 1);
			//					}
			//					else
			//					{
			//						++triangle_mult_count[i];
			//					}
			//				}
			//			}
			//		}
			//		else if (int_triangle_count > 1)
			//		{
			//			for (long i = 2 * L; i <= 1500000; i += L)
			//			{
			//				if (!triangle_mult_count.ContainsKey(i))
			//				{
			//					triangle_mult_count.Add(i, 2);
			//				}
			//				else
			//				{
			//					triangle_mult_count[i] = 2;
			//				}
			//			}
			//		}
			//	}

			//	if (triangle_mult_count.ContainsKey(L))
			//	{
			//		triangle_mult_count.Remove(L);
			//	}
			//}

			return singles_count.ToString();
		}
	}
}
