using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem033 : Problem
	{
		public override string CorrectAnswer { get { return "100"; } }

		protected override string CalculateSolution()
		{
			int nBig = 1, dBig = 1;
			for (int n = 10; n < 100; ++n)
			{
				for (int d = n + 1; d < 100; ++d)
				{
					int nCancel = 1;
					int dCancel = 1;
					if (n % 10 == d / 10)
					{
						nCancel = n / 10;
						dCancel = d % 10;
					}
					else if (n / 10 == d % 10)
					{
						nCancel = n % 10;
						dCancel = d / 10;
					}

					if (n * dCancel == d * nCancel)
					{
						nBig *= nCancel;
						dBig *= dCancel;
						for (int i = 2; i < 7; ++i)
						{
							while (nBig % i == 0 && dBig % i == 0)
							{
								nBig /= i;
								dBig /= i;
							}
						}
					}
				}
			}

			return dBig.ToString();
		}
	}
}
