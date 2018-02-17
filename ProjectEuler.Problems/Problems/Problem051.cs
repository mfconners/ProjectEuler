using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem051 : Problem
	{
		public override string CorrectAnswer { get { return "121313"; } }

		protected override string CalculateSolution()
		{
			int n = Primes.IndexOfPrimeAtMost(56003) + 1;
			List<int> digits = new List<int>();
			List<int> replaceDigits = new List<int>();

			while (true)
			{
				int prime = Primes.GetPrime(++n);

				digits.Clear();
				for (int p = prime; p > 0; p /= 10)
					digits.Add(p % 10);

				for (int replaced = 0; replaced <= 2; ++replaced)
				{
					replaceDigits.Clear();
					for (int i = 1; i < digits.Count; ++i)
						if (digits[i] == replaced)
							replaceDigits.Add(i);

					for (int replaceType = 1; replaceType < (1 << replaceDigits.Count); ++replaceType)
					{
						int missed = replaced;
						for (int replacement = replaced + 1; replacement < 10 && missed <= 2; ++replacement)
						{
							int testPrime = 0;
							for (int i = digits.Count - 1, j = replaceDigits.Count - 1; i >= 0; --i)
							{
								testPrime *= 10;
								if (j >= 0 && (replaceType & (1 << j)) != 0 && replaceDigits[j] == i)
									testPrime += replacement;
								else
									testPrime += digits[i];

								while (j > 0 && replaceDigits[j] >= i)
									--j;
							}
							if (!Primes.IsPrime(testPrime))
								++missed;
						}
						if (missed <= 2)
							return prime.ToString();
					}
				}
			}
		}
	}
}
