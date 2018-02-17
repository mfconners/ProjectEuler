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

			while (remainingDice.Count > 0)
			{
				int maxDice = diceProbability.Count - 1;
				int nextDiceSides = remainingDice.Dequeue();
				double diceSideRollProbability = 1.0 / nextDiceSides;
				List<double> nextDiceProbability = new List<double>(maxDice * nextDiceSides + 1);
				while (nextDiceProbability.Count < nextDiceProbability.Capacity)
					nextDiceProbability.Add(0.0);
				List<double> rollProbability = new List<double>();
				rollProbability.Add(1.0);
				for (int numDice = 1; numDice <= maxDice; ++numDice)
				{
					List<double> nextRollProbability = new List<double>(numDice * nextDiceSides + 1);
					while (nextRollProbability.Count < nextRollProbability.Capacity)
						nextRollProbability.Add(0.0);

					for (int diceRoll = 0; diceRoll < rollProbability.Count; ++diceRoll)
					{
						for (int newDieRoll = 1; newDieRoll <= nextDiceSides; ++newDieRoll)
						{
							int totalDiceRoll = diceRoll + newDieRoll;
							nextRollProbability[totalDiceRoll] += diceSideRollProbability * rollProbability[diceRoll];
						}
					}

					rollProbability = nextRollProbability;

					for (int diceRoll = 0; diceRoll < rollProbability.Count; ++diceRoll)
						nextDiceProbability[diceRoll] += diceProbability[numDice] * rollProbability[diceRoll];
				}
				diceProbability = nextDiceProbability;
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
