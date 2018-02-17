using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem036 : Problem
	{
		public override string CorrectAnswer { get { return "872187"; } }

		private static int oneMillion = 1000000;

		private static bool IsPalindrome(int number, int exp)
		{
			if (exp <= 0 || number < 0 || number % exp == 0)
				return false;
			if (number == 0)
				return true;

			int left = number, right = 0;
			while (left > right)
			{
				right = right * exp + left % exp;
				if (left == right)
					return true;

				left = left / exp;
				if (left == right)
					return true;
			}

			return false;
		}

		protected override string CalculateSolution()
		{
			long sum = 0;

			for (int number = 1; number < oneMillion; ++number)
				if (IsPalindrome(number, 10) && IsPalindrome(number, 2))
					sum += number;

			return sum.ToString();
		}
	}
}
