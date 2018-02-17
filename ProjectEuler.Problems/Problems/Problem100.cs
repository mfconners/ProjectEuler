using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem100 : Problem
	{
		public override string CorrectAnswer { get { return "756872327473"; } }

		private const UInt64 lower32 = 0xFFFFFFFF;
		private const UInt64 upper32 = lower32 << 32;

		protected override string CalculateSolution()
		{
			Int64 blueCount = 2, totalCount = 2;
			Int64 blue1 = 0, blue2 = 0, total1 = 0, total2 = 0;
			bool blueGood = false, totalGood = false;
			List<List<Int64>> factorsList = new List<List<Int64>>();
			List<Int64> curFactors = new List<Int64>();

			while (true)
			{
				#region Remove known factors from the blue.  If it doesn't have all the factors, mark it bad and increment it to the next number that can satisfy that factor...
				if (!blueGood)
				{
					blueGood = true;
					blue1 = blueCount;
					blue2 = blueCount - 1;
					for (int i = curFactors.Count - 1; blueGood && i >= 0; --i)
						if (blue1 % curFactors[i] == 0)
							blue1 /= curFactors[i];
						else if (blue2 % curFactors[i] == 0)
							blue2 /= curFactors[i];
						else
						{
							blueGood = false;
							blueCount = (blueCount / curFactors[i] + 1) * curFactors[i];
						}
				}
				#endregion

				#region Remove known factors from the total.  If it doesn't have all the factors, mark it bad and increment it to the next number that can satisfy that factor...
				if (!totalGood)
				{
					totalGood = true;
					total1 = totalCount;
					total2 = totalCount - 1;
					if (total1 % 2 == 0)
						total1 /= 2;
					else
						total2 /= 2;

					for (int i = curFactors.Count - 1; totalGood && i >= 0; --i)
						if (total1 % curFactors[i] == 0)
							total1 /= curFactors[i];
						else if (total2 % curFactors[i] == 0)
							total2 /= curFactors[i];
						else
						{
							totalGood = false;
							totalCount = (totalCount / curFactors[i] + 1) * curFactors[i];
						}
				}
				#endregion

				#region Check that if this is a solution.  If not, mark the smaller side bad.
				if (blueGood && totalGood)
				{
					if (Int64.MaxValue / blue1 < blue2 || Int64.MaxValue / total1 < total2)
						return blue1.ToString() + '|' + blue2.ToString() + '|' + total1.ToString() + '|' + total2.ToString();
					if (blue1 * blue2 > total1 * total2)
					{
						++totalCount;
						totalGood = false;
					}
					else if (blue1 * blue2 < total1 * total2)
					{
						++blueCount;
						blueGood = false;
					}
				}
				#endregion

				#region Add the next layer of factors.  Output the solution if we've gotten large enough...
				if (blueGood && totalGood)
				{
					if (totalCount > 1000000000000)
						return blueCount.ToString();

					List<Int64> newFactors = new List<Int64>();
					for (int p = 0;
							 blueGood && totalGood && (blue1 > 1 || blue2 > 1 || total1 > 1 || total2 > 1);
							 ++p)
					{
						int prime = Primes.GetPrime(p);
						while ((blue1 % prime == 0 || blue2 % prime == 0) &&
									 (total1 % prime == 0 || total2 % prime == 0))
						{
							if (blue1 % prime == 0)
								blue1 /= prime;
							else
								blue2 /= prime;

							if (total1 % prime == 0)
								total1 /= prime;
							else
								total2 /= prime;

							newFactors.Add(prime);
						}
					}
					factorsList.Add(newFactors);
					blueCount *= 5;
					blueGood = false;
					totalCount *= 5;
					totalGood = false;

					curFactors.Clear();
					for (int i = 0; i < factorsList.Count; ++i)
						if ((factorsList.Count + 2) % (i + 2) <= 1)
							curFactors.AddRange(factorsList[i]);
					curFactors.Sort();
					for (int i = curFactors.Count - 2, count = 1; i >= -1; --i)
						if (i >= 0 && curFactors[i] == curFactors[i + 1])
							++count;
						else
							while (count > 1)
							{
								curFactors[i + 1] *= curFactors[i + count];
								curFactors.RemoveAt(i + count);
								--count;
							}
					curFactors.Sort();
				}
				#endregion
			}
		}
	}
}
