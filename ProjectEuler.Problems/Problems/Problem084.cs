using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem084 : Problem
	{
		public override string CorrectAnswer { get { return "101524"; } }

		#region Space names defined as constants...
		private const int GO = 0;
		private const int A1 = 1;
		private const int CC1 = 2;
		private const int A2 = 3;
		private const int T1 = 4;
		private const int R1 = 5;
		private const int B1 = 6;
		private const int CH1 = 7;
		private const int B2 = 8;
		private const int B3 = 9;
		private const int JAIL = 10;
		private const int C1 = 11;
		private const int U1 = 12;
		private const int C2 = 13;
		private const int C3 = 14;
		private const int R2 = 15;
		private const int D1 = 16;
		private const int CC2 = 17;
		private const int D2 = 18;
		private const int D3 = 19;
		private const int FP = 20;
		private const int E1 = 21;
		private const int CH2 = 22;
		private const int E2 = 23;
		private const int E3 = 24;
		private const int R3 = 25;
		private const int F1 = 26;
		private const int F2 = 27;
		private const int U2 = 28;
		private const int F3 = 29;
		private const int G2J = 30;
		private const int G1 = 31;
		private const int G2 = 32;
		private const int CC3 = 33;
		private const int G3 = 34;
		private const int R4 = 35;
		private const int CH3 = 36;
		private const int H1 = 37;
		private const int T2 = 38;
		private const int H2 = 39;
		#endregion

		private static Dictionary<int, List<double>> InitSpaceProbabilities()
		{
			Dictionary<int, List<double>> spaceProbabilities = new Dictionary<int, List<double>>();
			for (int s = GO; s <= H2; ++s)
			{
				spaceProbabilities.Add(s, new List<double>());
				spaceProbabilities[s].Add((DICE_SIDES - 1) / DICE_SIDES);
				spaceProbabilities[s].Add((DICE_SIDES - 1) / DICE_SIDES / DICE_SIDES);
				spaceProbabilities[s].Add(1.0 / DICE_SIDES / DICE_SIDES);
			}

			return spaceProbabilities;
		}

		private static Dictionary<int, Dictionary<bool, double>> InitDiceProbabilities()
		{
			Dictionary<int, Dictionary<bool, double>> diceProbabilities = new Dictionary<int, Dictionary<bool, double>>();
			for (int d1 = 1; d1 <= DICE_SIDES; ++d1)
				for (int d2 = 1; d2 <= DICE_SIDES; ++d2)
				{
					int roll = d1 + d2;
					bool doubles = d1 == d2;
					if (!diceProbabilities.ContainsKey(d1 + d2))
						diceProbabilities.Add(roll, new Dictionary<bool, double>());
					if (!diceProbabilities[roll].ContainsKey(doubles))
						diceProbabilities[roll].Add(doubles, 0.0);
					diceProbabilities[roll][doubles] += 1.0 / DICE_SIDES / DICE_SIDES;
				}

			return diceProbabilities;
		}

		private static Dictionary<int, List<double>> BuildUpdatedSpaceProbabilities(Dictionary<int, List<double>> spaceProbabilities, Dictionary<int, Dictionary<bool, double>> diceProbabilities)
		{
			Dictionary<int, List<double>> updatedSpaceProbabilities = new Dictionary<int, List<double>>();
			for (int s = GO; s <= H2; ++s)
			{
				updatedSpaceProbabilities.Add(s, new List<double>());
				for (int prevDoubles = 0; prevDoubles < 3; ++prevDoubles)
					updatedSpaceProbabilities[s].Add(0);
			}

			for (int s = GO; s <= H2; ++s)
				foreach (int roll in diceProbabilities.Keys)
					for (int prevDoubles = 0; prevDoubles < 3; ++prevDoubles)
					{
						int nextSpace = (s + roll) % 40;

						if (diceProbabilities[roll].ContainsKey(false))
							updatedSpaceProbabilities[nextSpace][0] +=
								 spaceProbabilities[s][prevDoubles] * diceProbabilities[roll][false];

						if (diceProbabilities[roll].ContainsKey(true))
						{
							if (prevDoubles < 2)
								updatedSpaceProbabilities[nextSpace][prevDoubles + 1] +=
									 spaceProbabilities[s][prevDoubles] * diceProbabilities[roll][true];
							else
								updatedSpaceProbabilities[JAIL][0] +=
									 spaceProbabilities[s][2] * diceProbabilities[roll][true];
						}
					}

			for (int prevDoubles = 0; prevDoubles < 3; ++prevDoubles)
			{
				#region Correct Probabilities for Effects of Space 30 : GO TO JAIL
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[G2J][prevDoubles];
				updatedSpaceProbabilities[G2J][prevDoubles] = 0;
				#endregion
				#region Correct Probabilities for Effects of Spaces 7, 22 & 36 : CHANCE CARDS
				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[C1][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[E3][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[H2][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[R1][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[R2][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 8;
				updatedSpaceProbabilities[U1][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[T1][prevDoubles] += updatedSpaceProbabilities[CH1][prevDoubles] / 16;
				updatedSpaceProbabilities[CH1][prevDoubles] *= 6.0 / 16.0;

				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[C1][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[E3][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[H2][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[R1][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[R3][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 8;
				updatedSpaceProbabilities[U2][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[D3][prevDoubles] += updatedSpaceProbabilities[CH2][prevDoubles] / 16;
				updatedSpaceProbabilities[CH2][prevDoubles] *= 6.0 / 16.0;

				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[C1][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[E3][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[H2][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[R1][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[R1][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 8;
				updatedSpaceProbabilities[U1][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[CC3][prevDoubles] += updatedSpaceProbabilities[CH3][prevDoubles] / 16;
				updatedSpaceProbabilities[CH3][prevDoubles] *= 6.0 / 16.0;
				#endregion
				#region Correct Probabilities for Effects of Spaces 2, 17 & 33 : COMMUNITY CHEST CARDS
				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CC1][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CC1][prevDoubles] / 16;
				updatedSpaceProbabilities[CC1][prevDoubles] *= 7.0 / 8.0;

				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CC2][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CC2][prevDoubles] / 16;
				updatedSpaceProbabilities[CC2][prevDoubles] *= 7.0 / 8.0;

				updatedSpaceProbabilities[GO][prevDoubles] += updatedSpaceProbabilities[CC3][prevDoubles] / 16;
				updatedSpaceProbabilities[JAIL][0] += updatedSpaceProbabilities[CC3][prevDoubles] / 16;
				updatedSpaceProbabilities[CC3][prevDoubles] *= 7.0 / 8.0;
				#endregion
			}

			return updatedSpaceProbabilities;
		}



		private const int DICE_SIDES = 4;
		protected override string CalculateSolution()
		{
			Dictionary<int, List<double>> spaceProbabilities = InitSpaceProbabilities();

			Dictionary<int, Dictionary<bool, double>> diceProbabilities = InitDiceProbabilities();

			bool proceed = true;
			while (proceed)
			{
				Dictionary<int, List<double>> updatedSpaceProbabilities = BuildUpdatedSpaceProbabilities(spaceProbabilities, diceProbabilities);

				// Has any probability changed more than 1% from last round's estimate to this round's estimate?
				proceed = false;
				for (int s = GO; s <= H2; ++s)
					for (int prevDouble = 0; !proceed && prevDouble < 3; ++prevDouble)
					{
						double diff = spaceProbabilities[s][prevDouble] /
							 updatedSpaceProbabilities[s][prevDouble];
						if (diff < 1.0)
							diff = 1.0 / diff;
						if (diff > 1.0001)
							proceed = true;
					}

				spaceProbabilities = updatedSpaceProbabilities;
			}

			List<int> spaces = new List<int>();
			List<double> probabilities = new List<double>();

			for (int s = GO; s <= H2; ++s)
			{
				double totalProbability = spaceProbabilities[s][0] + spaceProbabilities[s][1] + spaceProbabilities[s][2];
				if (spaces.Count == 0)
				{
					spaces.Add(s);
					probabilities.Add(totalProbability);
				}
				else if (totalProbability > probabilities[0])
				{
					spaces.Insert(0, s);
					probabilities.Insert(0, totalProbability);
				}
				else if (spaces.Count == 1)
				{
					spaces.Add(s);
					probabilities.Add(totalProbability);
				}
				else if (totalProbability > probabilities[1])
				{
					spaces.Insert(1, s);
					probabilities.Insert(1, totalProbability);
				}
				else if (spaces.Count == 2)
				{
					spaces.Add(s);
					probabilities.Add(totalProbability);
				}
				else if (totalProbability > probabilities[2])
				{
					spaces.Insert(2, s);
					probabilities.Insert(2, totalProbability);
				}
			}

			string output = string.Empty;
			for (int i = 0; i < 3; ++i)
				output += spaces[i].ToString("00");

			return output;
		}
	}
}
