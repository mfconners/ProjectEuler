using System;
using System.IO;
using System.Collections.Generic;

namespace ProjectEuler.Problems
{
	class Problem042 : Problem
	{
		public override string CorrectAnswer { get { return "162"; } }

		private static char[] separators = { '\"', ',', '\r', '\n' };

		protected override string CalculateSolution()
		{
			int triangle_count = 0;
			HashSet<int> triangles = new HashSet<int>();
			int maxTriangle = 0;

			List<string> words_file = new List<string>(Properties.Resources.words.Split(separators, StringSplitOptions.RemoveEmptyEntries));
			foreach (string word in words_file)
			{
				int letter_sum = word.Length;
				foreach (char letter in word)
				{
					letter_sum += letter - 'A';
				}

				while (letter_sum > maxTriangle)
				{
					maxTriangle = (triangles.Count + 1) * (triangles.Count + 2) / 2;
					triangles.Add(maxTriangle);
				}

				if (triangles.Contains(letter_sum))
				{
					++triangle_count;
				}
			}

			return triangle_count.ToString();
		}
	}
}
