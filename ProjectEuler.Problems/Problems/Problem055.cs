using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem055 : Problem
	{
		public override string CorrectAnswer { get { return "249"; } }

		protected override string CalculateSolution()
		{
			int lychrel_count = 0;
			List<int> test = new List<int>();

			for (test.Add(1); test.Count < 5; ++test[0])
			{
				for (int digit = 0; digit < test.Count && test[digit] >= 10; ++digit)
				{
					if (digit + 1 < test.Count)
						++test[digit + 1];
					else
						test.Add(1);
					test[digit] %= 10;
				}

				List<int> last = new List<int>(test);
				bool palindrome_found = false;
				for (int count = 0; !palindrome_found && count < 50; ++count)
				{
					List<int> pal_test = new List<int>();
					for (int digit = 0; digit < last.Count; ++digit)
					{
						if (digit < pal_test.Count)
							pal_test[digit] += last[digit];
						else
							pal_test.Add(last[digit]);
						pal_test[digit] += last[last.Count - digit - 1];

						if (pal_test[digit] >= 10)
						{
							pal_test.Add(pal_test[digit] / 10);
							pal_test[digit] %= 10;
						}
					}

					palindrome_found = true;
					for (int digit = 0; palindrome_found && digit < pal_test.Count; ++digit)
					{
						if (pal_test[digit] != pal_test[pal_test.Count - digit - 1])
							palindrome_found = false;
					}

					last = pal_test;
				}

				if (!palindrome_found)
					++lychrel_count;
			}

			return lychrel_count.ToString();
		}
	}
}
