using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem679 : Problem
	{
		public override string CorrectAnswer { get { return "644997092988678"; } }

		private enum Letter : byte { A, E, F, R };
		private enum Word : byte { FREE = 0x1, FARE = 0x2, AREA = 0x4, REEF = 0x8 };
		private struct WordGrouping
		{
			public Letter letter0, letter1, letter2;
			public Word wordsComplete;

			public WordGrouping(Letter letter0 = Letter.A, Letter letter1 = Letter.A, Letter letter2 = Letter.A, Word wordsComplete = 0x0)
			{
				this.letter0 = letter0;
				this.letter1 = letter1;
				this.letter2 = letter2;
				this.wordsComplete = wordsComplete;
			}
		};

		private HashSet<WordGrouping> BuildGroupings()
		{
			HashSet<WordGrouping> groupings = new HashSet<WordGrouping>();
			for (WordGrouping group = new WordGrouping(); group.letter0 <= Letter.R; ++group.letter0)
			{
				for (group.letter1 = Letter.A; group.letter1 <= Letter.R; ++group.letter1)
				{
					for (group.letter2 = Letter.A; group.letter2 <= Letter.R; ++group.letter2)
					{
						for (group.wordsComplete = 0x0; group.wordsComplete <= (Word.AREA | Word.FARE | Word.FREE | Word.REEF); ++group.wordsComplete)
						{
							groupings.Add(group);
						}
					}
				}
			}

			return groupings;
		}

		private Dictionary<WordGrouping, List<WordGrouping>> BuildTransitions(HashSet<WordGrouping> groupings)
		{
			Dictionary<WordGrouping, List<WordGrouping>> transition = new Dictionary<WordGrouping, List<WordGrouping>>();
			foreach (WordGrouping from in groupings)
			{
				transition.Add(from, new List<WordGrouping>(4));

				for (WordGrouping to = new WordGrouping(from.letter1, from.letter2); to.letter2 <= Letter.R; ++to.letter2)
				{
					to.wordsComplete = from.wordsComplete;

					switch(from.letter0)
					{
						case Letter.A:
							if (to.letter0 == Letter.R && to.letter1 == Letter.E && to.letter2 == Letter.A)
							{
								to.wordsComplete |= Word.AREA;
								if (from.wordsComplete == to.wordsComplete)
									continue;
							}
							break;

						case Letter.E:
							break;

						case Letter.F:
							if (to.letter0 == Letter.A && to.letter1 == Letter.R && to.letter2 == Letter.E)
							{
								to.wordsComplete = from.wordsComplete | Word.FARE;
								if (from.wordsComplete == to.wordsComplete)
									continue;
							}
							else if (to.letter0 == Letter.R && to.letter1 == Letter.E && to.letter2 == Letter.E)
							{
								to.wordsComplete |= Word.FREE;
								if (from.wordsComplete == to.wordsComplete)
									continue;
							}
							break;

						case Letter.R:
							if (to.letter0 == Letter.E && to.letter1 == Letter.E && to.letter2 == Letter.F)
							{
								to.wordsComplete |= Word.REEF;
								if (from.wordsComplete == to.wordsComplete)
									continue;
							}
							break;
					};

					transition[from].Add(to);
				}
			}

			return transition;
		}

		private string f(int n)
		{
			HashSet<WordGrouping> groupings = BuildGroupings();
			Dictionary<WordGrouping, List<WordGrouping>> transition = BuildTransitions(groupings);

			Dictionary<WordGrouping, long> counts = new Dictionary<WordGrouping, long>();
			Dictionary<WordGrouping, long> prev_counts = new Dictionary<WordGrouping, long>();
			foreach (WordGrouping group in groupings)
			{
				prev_counts.Add(group, 0);
				if (group.wordsComplete == 0x0)
				{
					counts.Add(group, 1);
				}
				else
				{
					counts.Add(group, 0);
				}
			}

			for (int length = 3; length < n; ++length)
			{
				{ var temp_counts = counts; counts = prev_counts; prev_counts = temp_counts; }

				foreach (WordGrouping group in groupings)
				{
					counts[group] = 0;
				}

				foreach (WordGrouping prev_group in prev_counts.Keys)
					foreach (WordGrouping next_group in transition[prev_group])
						counts[next_group] += prev_counts[prev_group];
			}

			long total_count = 0;
			foreach (WordGrouping group in counts.Keys)
			{
				if (group.wordsComplete == (Word.AREA | Word.FARE | Word.FREE | Word.REEF))
				{
					total_count += counts[group];
				}
			}

			return total_count.ToString();
		}

		protected override string CalculateSolution()
		{
			return f(30);
		}
	}
}
