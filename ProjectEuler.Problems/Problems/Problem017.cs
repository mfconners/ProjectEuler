using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem017 : Problem
	{
		public override string CorrectAnswer { get { return "21124"; } }

		static private List<string> FillSingles()
		{
			List<string> singles = new List<string>();
			singles.Add("");
			singles.Add("one");
			singles.Add("two");
			singles.Add("three");
			singles.Add("four");
			singles.Add("five");
			singles.Add("six");
			singles.Add("seven");
			singles.Add("eight");
			singles.Add("nine");
			singles.Add("ten");
			singles.Add("eleven");
			singles.Add("twelve");
			singles.Add("thirteen");
			singles.Add("fourteen");
			singles.Add("fifteen");
			singles.Add("sixteen");
			singles.Add("seventeen");
			singles.Add("eighteen");
			singles.Add("nineteen");
			return singles;
		}

		static private List<string> FillTens()
		{
			List<string> tens = new List<string>();
			tens.Add("");
			tens.Add("");
			tens.Add("twenty");
			tens.Add("thirty");
			tens.Add("forty");
			tens.Add("fifty");
			tens.Add("sixty");
			tens.Add("seventy");
			tens.Add("eighty");
			tens.Add("ninety");
			return tens;
		}

		protected override string CalculateSolution()
		{
			int i;
			List<string> singles = FillSingles();
			List<string> tens = FillTens();
			string hundred = "hundred";
			string also = "and";
			string thousand = "thousand";
			long totalsize;

			totalsize = singles[1].Length + thousand.Length;
			totalsize += (999 - 100 + 1) * hundred.Length;
			totalsize += (999 - 100 + 1 - 9) * also.Length;
			for (i = 0; i < 10; i++)
			{
				totalsize += 100 * tens[i].Length;
				totalsize += 190 * singles[i].Length;
				totalsize += 10 * singles[i + 10].Length;
			}

			return totalsize.ToString();
		}
	}
}
