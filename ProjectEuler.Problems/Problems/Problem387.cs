using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem387 : Problem
	{
		public override string CorrectAnswer { get { return "696067597313468"; } }

		private struct HarshadTest
		{
			public long num;
			public long digit_sum;
			public bool is_strong;
		}

		//private const long max_prime_test = 10000;
		private const long max_prime_test = 696067597313468;
		private const long max_harshad = max_prime_test / 10;

		protected override string CalculateSolution()
		{
			Stack<HarshadTest> remaining = new Stack<HarshadTest>();
			long sum = 0;

			for (long init_harshad = 1; init_harshad < 10; ++init_harshad)
			{
				HarshadTest init;
				init.num = 10 * init_harshad;
				init.digit_sum = init_harshad;
				init.is_strong = false;
				remaining.Push(init);
			}

			while (remaining.Count > 0)
			{
				HarshadTest test_num = remaining.Pop();

				for (long max_test = test_num.num + 10; test_num.num < max_test; ++test_num.num, ++test_num.digit_sum)
				{
					if (Primes.IsPrime(test_num.num, false))
					{
						if (test_num.is_strong)
							sum += test_num.num;
					}
					else if (test_num.num % test_num.digit_sum == 0 && test_num.num < max_harshad)
					{
						HarshadTest new_test = test_num;
						new_test.num *= 10;
						new_test.is_strong = Primes.IsPrime(test_num.num / test_num.digit_sum, false);
						remaining.Push(new_test);
					}
				}
			}

			return sum.ToString();
		}
	}
}
