using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem052 : Problem
	{
		public override string CorrectAnswer { get { return "142857"; } }

		protected override string CalculateSolution()
		{
			List<char> basis = new List<char>();
			List<char> test_list = new List<char>();
			for (long test_num = 1; true; ++test_num)
			{
				bool matches = true;
				basis.Clear();
				for (long remainder = test_num; remainder > 0; remainder /= 10)
				{
					basis.Add((char)(remainder % 10));
				}
				basis.Sort();

				test_list.Clear();
				for (long remainder = 6 * test_num; remainder > 0; remainder /= 10)
				{
					test_list.Add((char)(remainder % 10));
				}
				test_list.Sort();
				if (test_list.Count > basis.Count)
				{
					test_num = 6 * test_num;
					continue;
				}
				for (int i = 0; matches && i < test_list.Count; ++i)
				{
					matches = (test_list[i] == basis[i]);
				}

				if (matches)
				{
					test_list.Clear();
					for (long remainder = 5 * test_num; remainder > 0; remainder /= 10)
					{
						test_list.Add((char)(remainder % 10));
					}
					test_list.Sort();
					for (int i = 0; matches && i < test_list.Count; ++i)
					{
						matches = (test_list[i] == basis[i]);
					}
				}

				if (matches)
				{
					test_list.Clear();
					for (long remainder = 4 * test_num; remainder > 0; remainder /= 10)
					{
						test_list.Add((char)(remainder % 10));
					}
					test_list.Sort();
					for (int i = 0; matches && i < test_list.Count; ++i)
					{
						matches = (test_list[i] == basis[i]);
					}
				}

				if (matches)
				{
					test_list.Clear();
					for (long remainder = 3 * test_num; remainder > 0; remainder /= 10)
					{
						test_list.Add((char)(remainder % 10));
					}
					test_list.Sort();
					for (int i = 0; matches && i < test_list.Count; ++i)
					{
						matches = (test_list[i] == basis[i]);
					}
				}

				if (matches)
				{
					test_list.Clear();
					for (long remainder = 2 * test_num; remainder > 0; remainder /= 10)
					{
						test_list.Add((char)(remainder % 10));
					}
					test_list.Sort();
					for (int i = 0; matches && i < test_list.Count; ++i)
					{
						matches = (test_list[i] == basis[i]);
					}
				}

				if (matches)
					return test_num.ToString();
			}
		}
	}
}
