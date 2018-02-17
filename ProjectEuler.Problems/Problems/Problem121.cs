using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem121 : Problem
	{
		public override string CorrectAnswer { get { return "2269"; } }

		const int NUM_TURNS = 15;

		protected override string CalculateSolution()
		{
			List<Int64> probability = new List<Int64>();
			probability.Add(1);

			while (probability.Count <= NUM_TURNS)
			{
				Int64 reds = probability.Count;
				List<Int64> nextProbability = new List<Int64>();

				while (nextProbability.Count <= probability.Count)
				{
					int index = nextProbability.Count;
					if (index == 0)
					{
						nextProbability.Add(reds * probability[0]);
					}
					else if (index < probability.Count)
					{
						nextProbability.Add(probability[index - 1] + reds * probability[index]);
					}
					else
					{
						nextProbability.Add(1);
					}
				}

				probability = nextProbability;
			}


			Int64 redWins = 0, blueWins = 0;
			for (int i = 0; i <= NUM_TURNS / 2; ++i)
				redWins += probability[i];
			for (int i = NUM_TURNS / 2 + 1; i <= NUM_TURNS; ++i)
				blueWins += probability[i];

			return ((redWins + blueWins) / blueWins).ToString();
		}
	}
}
