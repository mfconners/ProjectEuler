using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem389 : Problem
	{
		public override string CorrectAnswer { get { return "2406376.3623"; } }

		protected override string CalculateSolution()
		{
			Queue<int> remainingDice = new Queue<int>();
			remainingDice.Enqueue(4);
			remainingDice.Enqueue(6);
			remainingDice.Enqueue(8);
			remainingDice.Enqueue(12);
			remainingDice.Enqueue(20);

			List<double> diceProbability = new List<double>();
			diceProbability.Add(0.0);
			diceProbability.Add(1.0);

			List<double> nextDiceProbability = new List<double>();
			List<double> rollProbability = new List<double>();
			List<double> nextRollProbability = new List<double>();

			while (remainingDice.Count > 0)
			{
				int maxDice = diceProbability.Count - 1;
				int nextDiceSides = remainingDice.Dequeue();
				double diceSideRollProbability = 1.0 / nextDiceSides;

				while (nextDiceProbability.Count < maxDice * nextDiceSides + 1)
					nextDiceProbability.Add(0.0);

				rollProbability.Clear();
				rollProbability.Add(1.0);

				for (int numDice = 1; numDice <= maxDice; ++numDice)
				{
					while (nextRollProbability.Count < numDice * nextDiceSides + 1)
						nextRollProbability.Add(0.0);

					for (int diceRoll = 0; diceRoll < rollProbability.Count; ++diceRoll)
					{
						for (int newDieRoll = 1; newDieRoll <= nextDiceSides; ++newDieRoll)
						{
							int totalDiceRoll = diceRoll + newDieRoll;
							nextRollProbability[totalDiceRoll] += diceSideRollProbability * rollProbability[diceRoll];
						}
					}

					{
						List<double> temp;
						temp = rollProbability;
						rollProbability = nextRollProbability;
						nextRollProbability = temp;
						nextRollProbability.Clear();
					}

					for (int diceRoll = 0; diceRoll < rollProbability.Count; ++diceRoll)
						nextDiceProbability[diceRoll] += diceProbability[numDice] * rollProbability[diceRoll];
				}

				{
					List<double> temp;
					temp = diceProbability;
					diceProbability = nextDiceProbability;
					nextDiceProbability = temp;
					nextDiceProbability.Clear();
				}
			}

			double mean = 0.0;
			for (int diceroll = 1; diceroll < diceProbability.Count; ++diceroll)
				mean += diceroll * diceProbability[diceroll];

			double variance = 0.0;
			for (int diceroll = 0; diceroll < diceProbability.Count; ++diceroll)
			{
				double diff = diceroll - mean;
				variance += diceProbability[diceroll] * diff * diff;
			}

			return variance.ToString("0.0000");
		}
	}
}
