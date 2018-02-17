using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem016 : Problem
	{
		public override string CorrectAnswer { get { return "1366"; } }

		protected override string CalculateSolution()
		{
			List<int> bigDigits = new List<int>();
			bigDigits.Add(1);

			int digitSize = 10;
			while ((int.MaxValue) / 4 >= 10 * digitSize)
				digitSize *= 10;

			for (int count = 0; count < 500; ++count)
			{
				for (int i = bigDigits.Count - 1; i >= 0; --i)
				{
					bigDigits[i] *= 4;
					for (int j = 0; bigDigits[i + j] > digitSize; ++j)
					{
						if (i + j + 1 < bigDigits.Count)
						{
							bigDigits[i + j + 1] += bigDigits[i + j] / digitSize;
						}
						else
						{
							bigDigits.Add(bigDigits[i + j] / digitSize);
						}
						bigDigits[i + j] %= digitSize;
					}
				}
			}

			int sum = 0;
			for (int i = 0; i < bigDigits.Count; ++i)
			{
				while (bigDigits[i] > 0)
				{
					sum += bigDigits[i] % 10;
					bigDigits[i] /= 10;
				}
			}

			return sum.ToString();
		}
	}
}
