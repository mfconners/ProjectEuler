using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ProjectEuler.Problems
{
	class Problem044 : Problem
	{
		public override string CorrectAnswer { get { return "5482660"; } }

		static private long CalcPentagon(long index)
		{
			return index * (3 * index - 1) / 2;
		}

		static private void AddPentagon(List<long> pentagons, HashSet<long> pent_set)
		{
			pentagons.Add(CalcPentagon(pentagons.Count + 1));
			pent_set.Add(pentagons[pentagons.Count - 1]);
		}

		static private long GetPentagon(List<long> pentagons, HashSet<long> pent_set, int index)
		{
			while (index >= pentagons.Count)
				AddPentagon(pentagons, pent_set);

			return pentagons[index];
		}

		protected override string CalculateSolution()
		{
			List<long> pentagons = new List<long>();
			HashSet<long> pent_set = new HashSet<long>();
			List<int> tests = new List<int>();

			tests.Add(1);

			for (int pent = 0; true; ++pent)
			{
				long D = GetPentagon(pentagons, pent_set, pent);

				for (int test = 1; test < tests.Count || tests[tests.Count - 1] != 0; ++test)
				{
					while (test >= tests.Count)
						tests.Add(0);

					long Pj = GetPentagon(pentagons, pent_set, tests[test]);
					long Pk = GetPentagon(pentagons, pent_set, tests[test] + test);

					while (Pk - Pj < D)
					{
						Pj = GetPentagon(pentagons, pent_set, ++tests[test]);
						Pk = GetPentagon(pentagons, pent_set, tests[test] + test);
					}

					if (Pk - Pj == D)
					{
						long sum = Pj + Pk;

						if (sum > pentagons[pentagons.Count - 1])
						{
							int min = pentagons.Count;
							int max = 2 * min;
							long pent_max = CalcPentagon(max);

							if (sum == CalcPentagon(min))
								return D.ToString();

							while (pent_max < sum)
							{
								pent_max = CalcPentagon(max = 2 * max);
							}

							if (sum == pent_max)
								return D.ToString();

							while (max > min + 1)
							{
								int mid = (min + max) / 2;
								long pent_mid = CalcPentagon(mid);

								if (sum == pent_mid)
									return D.ToString();
								else if (sum < pent_mid)
									max = mid;
								else
									min = mid;
							}
						}
						else if (pent_set.Contains(sum))
						{
							return D.ToString();
						}
					}
				}
			}
		}
	}
}
