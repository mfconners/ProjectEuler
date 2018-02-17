using System;
using System.Collections;
using System.Linq;
using System.Numerics;

namespace ProjectEuler.MathExtensions
{
	internal static class BigIntegerOps
	{
		public static UInt64 SumDigits(BigInt n)
		{
			return n.SumDigits();
		}

		public static bool IsPalindrome(BigInteger n)
		{
			string front = n.ToString();
			string back = string.Empty;

			while (front.Length > back.Length + 1)
			{
				back += front.Last();
				front = front.Substring(0, front.Length - 1);
			}
			if (front.Length > back.Length)
				front = front.Substring(0, front.Length - 1);

			return (front == back);
		}

		private static BitArray usedDigits = new BitArray(10);
		private static BitArray usedDigitsTest = new BitArray(10);

		public static bool IsPanDigital(int n)
		{
			int count = 0;
			for (usedDigits.SetAll(false), usedDigitsTest.SetAll(false); !usedDigits[0] && n > 0; n /= 10)
			{
				int leastSignificant = n % 10;
				if (usedDigits[leastSignificant] || leastSignificant == 0)
				{
					usedDigits[0] = true;
					break;
				}
				else
				{
					usedDigits[leastSignificant] = true;
					usedDigitsTest[++count] = true;
				}
			}

			for (int i = 0; i < 10; ++i)
				if (usedDigits[i] != usedDigitsTest[i])
					return false;
			return true;
		}
	}
}
