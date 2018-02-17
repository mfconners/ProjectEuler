using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem054 : Problem
	{
		private enum WinStatus
		{
			P1_Win,
			P2_Win,
			Tie,
			Unknown
		};

		public override string CorrectAnswer { get { return "376"; } }

		private static readonly char[] separator = { ' ' };

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			int P1_win_count = 0;
			List<char> isFlush = new List<char>(2);
			List<bool> isStraight = new List<bool>(2);
			List<List<byte>> cards = new List<List<byte>>(2);
			List<List<KeyValuePair<byte, byte>>> multiples = new List<List<KeyValuePair<byte, byte>>>(2);
			for (int player = 0; player < 2; ++player)
			{
				isFlush.Add(' ');
				isStraight.Add(true);
				cards.Add(new List<byte>(5));
				for (int card = 0; card < 5; ++card)
					cards[player].Add(0);
				multiples.Add(new List<KeyValuePair<byte, byte>>(2));
			}

			string[] poker_file = Properties.Resources.poker.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);
			foreach(string line in poker_file)
			{
				List<string> card_strings = new List<string>(line.Split(separator, StringSplitOptions.RemoveEmptyEntries));
				if (card_strings.Count != 10)
					throw new FileFormatException();

				for (int player = 0; player < 2; ++player)
				{
					for (int card = 0, card_index = 5 * player; card < 5; card_index = ++card + 5 * player)
					{
						if (card == 0)
						{
							isFlush[player] = card_strings[card_index][1];
						}
						else if (card_strings[card_index][1] != isFlush[player])
						{
							isFlush[player] = ' ';
						}

						switch (card_strings[card_index][0])
						{
							case 'T':
								cards[player][card] = 10;
								break;
							case 'J':
								cards[player][card] = 11;
								break;
							case 'Q':
								cards[player][card] = 12;
								break;
							case 'K':
								cards[player][card] = 13;
								break;
							case 'A':
								cards[player][card] = 14;
								break;
							default:
								if (card_strings[card_index][0] < '0' || card_strings[card_index][0] > '9')
									throw new FileFormatException();
								cards[player][card] = (byte)(card_strings[card_index][0] - '0');
								break;
						}
					}
				}

				if (WhoWins(cards, isFlush, isStraight, multiples) == WinStatus.P1_Win)
					++P1_win_count;
			}

			return P1_win_count.ToString();
		}

		static private WinStatus WhoWins(List<List<byte>> cards, List<char> isFlush, List<bool> isStraight, List<List<KeyValuePair<byte, byte>>> multiples)
		{
			for (int player = 0; player < 2; ++player)
			{
				cards[player].Sort();
				isStraight[player] = true;
				for (int card = 0; isStraight[player] && card < 4; ++card)
				{
					isStraight[player] = cards[player][card] + 1 == cards[player][card + 1];
				}
			}

			WinStatus status = CheckStraightFlush(cards, isFlush, isStraight);
			if (status != WinStatus.Unknown)
				return status;

			for (int player = 0; player < 2; ++player)
			{
				multiples[player].Clear();
				for (int card = 0; card < 4; ++card)
				{
					byte multiple_count;
					for (multiple_count = 1; card < 4 && cards[player][card] == cards[player][card + 1]; ++card)
						++multiple_count;

					if (multiple_count > 1)
					{
						multiples[player].Add(new KeyValuePair<byte, byte>(multiple_count, cards[player][card]));
					}
				}
			}

			status = CheckFourOfAKind(cards, multiples);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckFullHouse(multiples);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckFlush(cards, isFlush);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckStraight(cards, isStraight);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckThreeOfAKind(multiples);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckTwoPair(cards, multiples);
			if (status != WinStatus.Unknown)
				return status;

			status = CheckOnePair(cards, multiples);
			if (status != WinStatus.Unknown)
				return status;

			return BreakTie(cards);
		}

		static private WinStatus BreakTie(List<List<byte>> cards)
		{
			for (int card = 4; card >= 0; --card)
			{
				if (cards[0][card] > cards[1][card])
					return WinStatus.P1_Win;
				else if (cards[0][card] < cards[1][card])
					return WinStatus.P2_Win;
			}
			return WinStatus.Tie;
		}

		static private WinStatus CheckStraightFlush(List<List<byte>> cards, List<char> isFlush, List<bool> isStraight)
		{
			if ((isFlush[0] != ' ' && isStraight[0]) || (isFlush[1] != ' ' && isStraight[1]))
			{
				if (isFlush[0] != ' ' && isStraight[0] && isFlush[1] != ' ' && isStraight[1])
				{
					return BreakTie(cards);
				}
				else if (isFlush[0] != ' ' && isStraight[0])
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckFourOfAKind(List<List<byte>> cards, List<List<KeyValuePair<byte, byte>>> multiples)
		{
			if ((multiples[0].Count == 1 && multiples[0][0].Key == 4) ||
				(multiples[1].Count == 1 && multiples[1][0].Key == 4))
			{
				if ((multiples[0].Count == 1 && multiples[0][0].Key == 4) &&
				(multiples[1].Count == 1 && multiples[1][0].Key == 4))
				{
					if (multiples[0][0].Value > multiples[1][0].Value)
						return WinStatus.P1_Win;
					else if (multiples[0][0].Value < multiples[1][0].Value)
						return WinStatus.P2_Win;
					else
						return BreakTie(cards);
				}
				else if (multiples[0].Count == 1 && multiples[0][0].Key == 4)
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckFullHouse(List<List<KeyValuePair<byte, byte>>> multiples)
		{
			if ((multiples[0].Count == 2 && (multiples[0][0].Key == 3 || multiples[0][1].Key == 3)) ||
				(multiples[1].Count == 2 && (multiples[1][0].Key == 3 || multiples[1][1].Key == 3)))
			{
				if ((multiples[0].Count == 2 && (multiples[0][0].Key == 3 || multiples[0][1].Key == 3)) &&
				(multiples[1].Count == 2 && (multiples[1][0].Key == 3 || multiples[1][1].Key == 3)))
				{
					int P1_high, P2_high;

					if (multiples[0][0].Key == 3)
						P1_high = multiples[0][0].Value;
					else
						P1_high = multiples[0][0].Value;

					if (multiples[0][0].Key == 3)
						P2_high = multiples[1][0].Value;
					else
						P2_high = multiples[1][0].Value;

					if (P1_high > P2_high)
						return WinStatus.P1_Win;
					else
						return WinStatus.P2_Win;
				}
				else if (multiples[0].Count == 2 && (multiples[0][0].Key == 3 || multiples[0][1].Key == 3))
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckFlush(List<List<byte>> cards, List<char> isFlush)
		{
			if (isFlush[0] != ' ' || isFlush[1] != ' ')
			{
				if (isFlush[0] != ' ' && isFlush[1] != ' ')
				{
					return BreakTie(cards);
				}
				else if (isFlush[0] != ' ')
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckStraight(List<List<byte>> cards, List<bool> isStraight)
		{
			if (isStraight[0] || isStraight[1])
			{
				if (isStraight[0] && isStraight[1])
				{
					return BreakTie(cards);
				}
				else if (isStraight[0])
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckThreeOfAKind(List<List<KeyValuePair<byte, byte>>> multiples)
		{
			if ((multiples[0].Count > 0 && multiples[0][0].Key == 3) ||
				(multiples[1].Count > 0 && multiples[1][0].Key == 3))
			{
				if ((multiples[0].Count > 0 && multiples[0][0].Key == 3) &&
				(multiples[1].Count > 0 && multiples[1][0].Key == 3))
				{
					if (multiples[0][0].Value > multiples[1][0].Value)
						return WinStatus.P1_Win;
					else
						return WinStatus.P2_Win;
				}
				else if (multiples[0].Count > 0 && multiples[0][0].Key == 3)
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckTwoPair(List<List<byte>> cards, List<List<KeyValuePair<byte, byte>>> multiples)
		{
			if (multiples[0].Count == 2 || multiples[1].Count == 2)
			{
				if (multiples[0].Count == 2 && multiples[1].Count == 2)
				{
					int P1_high, P2_high, P1_low, P2_low;

					if (multiples[0][0].Value > multiples[0][1].Value)
					{
						P1_high = multiples[0][0].Value;
						P1_low = multiples[0][1].Value;
					}
					else
					{
						P1_high = multiples[0][1].Value;
						P1_low = multiples[0][0].Value;
					}

					if (multiples[1][0].Value > multiples[1][1].Value)
					{
						P2_high = multiples[1][0].Value;
						P2_low = multiples[1][1].Value;
					}
					else
					{
						P2_high = multiples[1][1].Value;
						P2_low = multiples[1][0].Value;
					}

					if (P1_high > P2_high)
						return WinStatus.P1_Win;
					else if (P1_high < P2_high)
						return WinStatus.P2_Win;
					else if (P1_low < P2_low)
						return WinStatus.P1_Win;
					else if (P1_low < P2_low)
						return WinStatus.P2_Win;
					else
						return BreakTie(cards);
				}
				else if (multiples[0].Count == 2)
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}

		static private WinStatus CheckOnePair(List<List<byte>> cards, List<List<KeyValuePair<byte, byte>>> multiples)
		{
			if (multiples[0].Count == 1 || multiples[1].Count == 1)
			{
				if (multiples[0].Count == 1 && multiples[1].Count == 1)
				{
					if (multiples[0][0].Value > multiples[1][0].Value)
						return WinStatus.P1_Win;
					else if (multiples[0][0].Value < multiples[1][0].Value)
						return WinStatus.P2_Win;
					else
						return BreakTie(cards);
				}
				else if (multiples[0].Count == 1)
				{
					return WinStatus.P1_Win;
				}
				else
				{
					return WinStatus.P2_Win;
				}
			}

			return WinStatus.Unknown;
		}
	}
}
