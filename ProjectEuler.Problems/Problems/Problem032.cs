using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem032 : Problem
	{
		public override string CorrectAnswer { get { return "45228"; } }

		protected override string CalculateSolution()
		{
			HashSet<int> products = new HashSet<int>();

			for (int multiplicand = 1; multiplicand * multiplicand * (multiplicand + 1) * (multiplicand + 1) <= 999999999; ++multiplicand)
			{
				HashSet<int> digits = new HashSet<int>();
				bool goodDigits = true;
				for (int remainingDigits = multiplicand; goodDigits && remainingDigits > 0; remainingDigits /= 10)
				{
					int nextDigit = remainingDigits % 10;
					goodDigits = !digits.Contains(nextDigit) && nextDigit != 0;
					if (goodDigits)
					{
						digits.Add(nextDigit);
					}
				}
				if (goodDigits)
				{
					for (int multiplier = multiplicand + 1; multiplicand * multiplicand * multiplier * multiplier < 999999999; ++multiplier, goodDigits = true)
					{
						int product = multiplicand * multiplier;
						if (products.Contains(product))
							continue;

						HashSet<int> digitsUnion = new HashSet<int>(digits);

						for (int remainingDigits = multiplier; goodDigits && remainingDigits > 0; remainingDigits /= 10)
						{
							int nextDigit = remainingDigits % 10;
							goodDigits = !digitsUnion.Contains(nextDigit) && nextDigit != 0;
							if (goodDigits)
							{
								digitsUnion.Add(nextDigit);
							}
						}

						if (goodDigits)
						{
							for (int remainingDigits = multiplicand * multiplier; goodDigits && remainingDigits > 0; remainingDigits /= 10)
							{
								int nextDigit = remainingDigits % 10;
								goodDigits = !digitsUnion.Contains(nextDigit) && nextDigit != 0;
								if (goodDigits)
								{
									digitsUnion.Add(nextDigit);
								}
							}

							if (goodDigits && digitsUnion.Count == 9)
							{
								products.Add(product);
							}
						}
					}
				}
			}

			int sum = 0;
			foreach (int product in products)
			{
				sum += product;
			}

			return sum.ToString();
		}
	}
}
