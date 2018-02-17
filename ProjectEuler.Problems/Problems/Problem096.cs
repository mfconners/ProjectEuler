using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem096 : Problem
	{
		public override string CorrectAnswer { get { return "24702"; } }

		private class SudokuOption
		{
			private int _position;
			public int Position { get { return _position; } }
			static public int GetPosition(int row, int column)
			{
				return 9 * row + column;
			}
			private int _number;
			public int Number { get { return _number; } }

			public int Row
			{
				get { return GetRow(_position); }
			}
			static public int GetRow(int position)
			{
				return position / 9;
			}

			public int Column
			{
				get { return GetColumn(_position); }
			}
			static public int GetColumn(int position)
			{
				return position % 9;
			}

			public int Box
			{
				get { return GetBox(_position); }
			}
			static public int GetBox(int position)
			{
				return 3 * (position / 27) + (position % 9) / 3;
			}

			public SudokuOption(int row, int column, int n)
			{
				_position = GetPosition(row, column);
				_number = n;
			}

			public SudokuOption(int p, int n)
			{
				_position = p;
				_number = n;
			}

			public override bool Equals(object obj)
			{
				SudokuOption option = obj as SudokuOption;
				if (obj != null)
				{
					return option.Number == Number && option.Position == Position;
				}

				return false;
			}

			public override int GetHashCode()
			{
				return 81 * Number + Position;
			}

			public override string ToString()
			{
				return '[' + Row.ToString() + ", " + Column.ToString() + "]:   #" + Number.ToString();
			}
		}

		private class Sudoku
		{
			List<LinkedList<SudokuOption>> _options =
					new List<LinkedList<SudokuOption>>(81);

			private int[] _rowDigits = new int[81];
			private int[] _colDigits = new int[81];
			private int[] _boxDigits = new int[81];
			private BitArray _resolvedPositions;
			private BitArray _resolvedRowDigits;
			private BitArray _resolvedColumnDigits;
			private BitArray _resolvedBoxDigits;
			private int _unresolvedCount = 4 * 81;

			Sudoku()
			{
				for (int i = 0; i < 81; ++i)
				{
					_options.Add(new LinkedList<SudokuOption>());
					_rowDigits[i] = 0;
					_colDigits[i] = 0;
					_boxDigits[i] = 0;
				}
				_resolvedPositions = new BitArray(81);
				_resolvedRowDigits = new BitArray(81);
				_resolvedColumnDigits = new BitArray(81);
				_resolvedBoxDigits = new BitArray(81);
			}

			Sudoku(Sudoku original)
			{
				for (int i = 0; i < 81; ++i)
				{
					_options.Add(new LinkedList<SudokuOption>(original._options[i]));
					_rowDigits[i] = original._rowDigits[i];
					_colDigits[i] = original._colDigits[i];
					_boxDigits[i] = original._boxDigits[i];
				}
				_resolvedPositions = new BitArray(original._resolvedPositions);
				_resolvedRowDigits = new BitArray(original._resolvedRowDigits);
				_resolvedColumnDigits = new BitArray(original._resolvedColumnDigits);
				_resolvedBoxDigits = new BitArray(original._resolvedBoxDigits);
				_unresolvedCount = original._unresolvedCount;
			}

			public static Sudoku Load(List<string> sudoku_file, int start_row)
			{
				int row = 0, end_row = start_row + 10, position = 0;
				string line;
				Sudoku loaded = new Sudoku();
				for (row = start_row + 1; row < end_row && row < sudoku_file.Count; ++row)
				{
					line = sudoku_file[row];
					if (line.Length != 9)
						throw new InvalidDataException(
								"Input file has lines of incorrect length: " + line.Length);
					for (int column = 0; column < 9; ++column, ++position)
					{
						if (line[column] == '0')
						{
							for (int digit = 1; digit <= 9; ++digit)
							{
								SudokuOption option = new SudokuOption(position, digit);
								loaded.Add(option);
							}
						}
						else if (line[column] >= '1' && line[column] <= '9')
						{
							SudokuOption option = new SudokuOption(position, line[column] - '0');
							loaded.Add(option);
						}
						else
							throw new InvalidDataException(
									"Input file has non-digit in the file: " + line[column]);
					}
				}
				return loaded;
			}

			private void Select(SudokuOption option)
			{
				int position = option.Position;
				int row = option.Row;
				int column = option.Column;
				int box = option.Box;
				int boxrow = 3 * (box / 3);
				int boxcolumn = 3 * (box % 3);
				LinkedList<SudokuOption> subOptions;

				subOptions = _options[position];
				while (subOptions.Count > 1)
					if (subOptions.Last() == option)
						Remove(subOptions.First());
					else
						Remove(subOptions.Last());

				for (int i = 0; i < 9; ++i)
				{
					if (i != column)
					{
						subOptions = _options[SudokuOption.GetPosition(row, i)];
						foreach (SudokuOption curOption in subOptions)
							if (option.Number == curOption.Number)
							{
								Remove(curOption);
								break;
							}
					}
					if (i != row)
					{
						subOptions = _options[SudokuOption.GetPosition(i, column)];
						foreach (SudokuOption curOption in subOptions)
							if (option.Number == curOption.Number)
							{
								Remove(curOption);
								break;
							}
					}
					if (i / 3 != row % 3 || i % 3 != column % 3)
					{
						subOptions = _options[
								SudokuOption.GetPosition(boxrow + i / 3, boxcolumn + i % 3)];
						foreach (SudokuOption curOption in subOptions)
							if (option.Number == curOption.Number)
							{
								Remove(curOption);
								break;
							}
					}
				}

				if (!_resolvedPositions[position])
				{
					_resolvedPositions[position] = true;
					--_unresolvedCount;
				}

				int digitIndex = 9 * (option.Number - 1);

				int rowIndex = digitIndex + row;
				if (!_resolvedRowDigits[rowIndex])
				{
					_resolvedRowDigits[rowIndex] = true;
					--_unresolvedCount;
				}

				int columnIndex = digitIndex + column;
				if (!_resolvedColumnDigits[columnIndex])
				{
					_resolvedColumnDigits[columnIndex] = true;
					--_unresolvedCount;
				}

				int boxIndex = digitIndex + box;
				if (!_resolvedBoxDigits[boxIndex])
				{
					_resolvedBoxDigits[boxIndex] = true;
					--_unresolvedCount;
				}
			}

			private bool ResolveAllSingles()
			{
				bool keepItUp;
				do
				{
					keepItUp = false;

					for (int i = 0; i < 81; ++i)
					{
						#region Search positions with count 1
						if (!_resolvedPositions[i])
						{
							if (_options[i].Count == 1)
							{
								Select(_options[i].First());
								keepItUp = true;
								break;
							}
							else if (_options[i].Count == 0)
								return false;
						}
						#endregion

						#region Search rows with bigDigits having only 1 possible location
						if (!_resolvedRowDigits[i])
						{
							if (_rowDigits[i] == 1)
							{
								int row = i % 9;
								int digit = i / 9 + 1;
								bool foundDigit = false;
								for (int column = 0; !foundDigit; ++column)
								{
									int position = SudokuOption.GetPosition(row, column);
									foreach (var cur in _options[position])
										if (cur.Number == digit)
										{
											Select(cur);
											foundDigit = true;
											keepItUp = true;
											break;
										}
								}
							}
							else if (_rowDigits[i] == 0)
								return false;
						}
						#endregion

						#region Search columns with bigDigits having only 1 possible location
						if (!_resolvedColumnDigits[i])
						{
							if (_colDigits[i] == 1)
							{
								int column = i % 9;
								int digit = i / 9 + 1;
								bool foundDigit = false;
								for (int row = 0; !foundDigit; ++row)
								{
									int position = SudokuOption.GetPosition(row, column);
									foreach (var cur in _options[position])
										if (cur.Number == digit)
										{
											Select(cur);
											foundDigit = true;
											keepItUp = true;
											break;
										}
								}
							}
							else if (_colDigits[i] == 0)
								return false;
						}
						#endregion

						#region Search boxes with bigDigits having only 1 possible location
						if (!_resolvedBoxDigits[i])
						{
							if (_boxDigits[i] == 1)
							{
								int box = i % 9;
								int row = 3 * (box / 3);
								int column = 3 * (box % 3);
								int digit = i / 9 + 1;
								bool foundDigit = false;
								for (int j = 0; !foundDigit; ++j)
								{
									int position = SudokuOption.GetPosition(row + j / 3, column + j % 3);
									foreach (var cur in _options[position])
										if (cur.Number == digit)
										{
											Select(cur);
											foundDigit = true;
											keepItUp = true;
											break;
										}
								}
							}
							else if (_boxDigits[i] == 0)
								return false;
						}
						#endregion
					}
				} while (keepItUp);

				return true;
			}

			public Sudoku Solve()
			{
				if (!ResolveAllSingles())
					return null;

				if (_unresolvedCount > 0)
				{
					#region Find the next selection area
					int minCount = int.MaxValue;
					int row = -1;
					int column = -1;
					int box = -1;
					int digit = -1;
					int index = -1;

					for (int i = 0; minCount > 2 && i < 81; ++i)
					{
						if (!_resolvedPositions[i] && _options[i].Count < minCount)
						{
							minCount = _options[i].Count;
							if (minCount == 0)
								return null;
							row = -1;
							column = -1;
							box = -1;
							digit = -1;
							index = i;
						}
						if (!_resolvedRowDigits[i] && _rowDigits[i] < minCount)
						{
							minCount = _rowDigits[i];
							if (minCount == 0)
								return null;
							row = i % 9;
							column = -1;
							box = -1;
							digit = i / 9 + 1;
							index = i;
						}
						if (!_resolvedColumnDigits[i] && _colDigits[i] < minCount)
						{
							minCount = _colDigits[i];
							if (minCount == 0)
								return null;
							row = -1;
							column = i % 9;
							box = -1;
							digit = i / 9 + 1;
							index = i;
						}
						if (!_resolvedBoxDigits[i] && _boxDigits[i] < minCount)
						{
							minCount = _boxDigits[i];
							if (minCount == 0)
								return null;
							row = -1;
							column = -1;
							box = i % 9;
							digit = i / 9 + 1;
							index = i;
						}
					}
					#endregion

					#region Find the selection options
					LinkedList<SudokuOption> selections;

					if (digit < 0)
						selections = _options[index];
					else
					{
						selections = new LinkedList<SudokuOption>();
						if (box >= 0)
						{
							row = 3 * (box / 3);
							column = 3 * (box % 3);
							for (int i = 0; i < 9 && minCount > 0; ++i)
							{
								int position = SudokuOption.GetPosition(row + i / 3, column + i % 3);
								foreach (SudokuOption curOption in _options[position])
								{
									if (digit == curOption.Number)
									{
										selections.AddLast(curOption);
										--minCount;
										break;
									}
								}
							}
						}
						else if (row >= 0)
						{
							for (column = 0; minCount > 0 && column < 9; ++column)
							{
								int position = SudokuOption.GetPosition(row, column);
								foreach (SudokuOption curOption in _options[position])
								{
									if (digit == curOption.Number)
									{
										selections.AddLast(curOption);
										--minCount;
										break;
									}
								}
							}
						}
						else if (column >= 0)
						{
							for (row = 0; minCount > 0 && row < 9; ++row)
							{
								int position = SudokuOption.GetPosition(row, column);
								foreach (SudokuOption curOption in _options[position])
								{
									if (digit == curOption.Number)
									{
										selections.AddLast(curOption);
										--minCount;
										break;
									}
								}
							}
						}
					}

					if (selections.Count <= 0)
						return null;
					#endregion

					#region Find a solution from the selection options
					foreach (SudokuOption curOption in selections)
					{
						Sudoku solution = new Sudoku(this);
						solution.Select(curOption);
						solution = solution.Solve();
						if (solution != null)
							return solution;
					}

					return null;
					#endregion
				}
				else if (_unresolvedCount == 0)
				{
					return this;
				}
				else
				{
					return null;
				}
			}

			public string GetNumber(int position)
			{
				string possibleNumbers = string.Empty;
				foreach (SudokuOption ps in _options[position])
					possibleNumbers += ps.Number.ToString();
				return possibleNumbers;
			}

			public void Add(SudokuOption s)
			{
				int position = s.Position;
				int numberIndex = 9 * (s.Number - 1);
				int rowIndex = numberIndex + s.Row;
				int columnIndex = numberIndex + s.Column;
				int boxIndex = numberIndex + s.Box;

				_options[position].AddLast(s);
				if (_resolvedPositions[position])
				{
					_resolvedPositions[position] = false;
					++_unresolvedCount;
				}

				++_rowDigits[rowIndex];
				++_colDigits[columnIndex];
				++_boxDigits[boxIndex];

				if (_resolvedRowDigits[rowIndex])
				{
					_resolvedRowDigits[rowIndex] = false;
					++_unresolvedCount;
				}

				if (_resolvedColumnDigits[columnIndex])
				{
					_resolvedColumnDigits[columnIndex] = false;
					++_unresolvedCount;
				}

				if (_resolvedBoxDigits[boxIndex])
				{
					_resolvedBoxDigits[boxIndex] = false;
					++_unresolvedCount;
				}
			}

			public void Remove(SudokuOption s)
			{
				int position = s.Position;
				int numberIndex = 9 * (s.Number - 1);
				int rowIndex = numberIndex + s.Row;
				int columnIndex = numberIndex + s.Column;
				int boxIndex = numberIndex + s.Box;

				if (_options[position].Contains(s))
				{
					if (s == _options[position].Last())
						_options[position].RemoveLast();
					else if (s == _options[position].First())
						_options[position].RemoveFirst();
					else
						_options[position].Remove(s);

					--_rowDigits[rowIndex];
					--_colDigits[columnIndex];
					--_boxDigits[boxIndex];
				}
				else
					throw new ArgumentOutOfRangeException("s", "Not in _options");

				if (_options[position].Count == 0)
					return;
			}

			public bool Verify()
			{
				bool verified = true;

				int count = 0;

				for (int i = 0; i < 81; ++i)
				{
					if (_resolvedPositions[i])
					{
						if (_options[i].Count != 1)
							verified = false;
					}
					else
						++count;

					if (_resolvedRowDigits[i])
					{
						if (_rowDigits[i] != 1)
							verified = false;
					}
					else
						++count;

					if (_resolvedColumnDigits[i])
					{
						if (_colDigits[i] != 1)
							verified = false;
					}
					else
						++count;

					if (_resolvedBoxDigits[i])
					{
						if (_boxDigits[i] != 1)
							verified = false;
					}
					else
						++count;
				}

				if (_unresolvedCount != count)
					verified = false;

				return verified;
			}

			public bool VerifySolution()
			{
				bool verified = Verify();

				if (_unresolvedCount != 0)
					verified = false;

				BitArray digits = new BitArray(9);

				for (int row = 0; row < 81; row += 9)
				{
					digits.SetAll(false);
					for (int position = row; position < row + 9; ++position)
					{
						if (_options[position].Count != 1)
							verified = false;
						else
						{
							if (digits[_options[position].First().Number - 1])
								verified = false;
							digits[_options[position].First().Number - 1] = true;
						}
					}
				}

				for (int column = 0; column < 9; ++column)
				{
					digits.SetAll(false);
					for (int position = column; position < 81; position += 9)
					{
						if (_options[position].Count != 1)
							verified = false;
						else
						{
							if (digits[_options[position].First().Number - 1])
								verified = false;
							digits[_options[position].First().Number - 1] = true;
						}
					}
				}

				for (int box = 0; box < 9; ++box)
				{
					digits.SetAll(false);
					int start = 27 * (box / 3) + 3 * (box % 3);
					for (int j = 0; j < 9; ++j)
					{
						int position = start + 9 * (j / 3) + j % 3;
						if (_options[position].Count != 1)
							verified = false;
						else
						{
							if (digits[_options[position].First().Number - 1])
								verified = false;
							digits[_options[position].First().Number - 1] = true;
						}
					}
				}

				return verified;
			}
		}

		private static readonly char[] newline_separators = { '\r', '\n' };

		protected override string CalculateSolution()
		{
			int sum = 0, count = 0;

			List<string> sudoku_file = new List<string>(Properties.Resources.sudoku.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries));

			for (int i = 0; i < sudoku_file.Count; i += 10)
			{
				++count;
				Sudoku sudoku = Sudoku.Load(sudoku_file, i);
				sudoku = sudoku.Solve();
				if (sudoku != null && sudoku.VerifySolution())
				{
					string number = sudoku.GetNumber(SudokuOption.GetPosition(0, 0));
					if (number.Length == 1)
						sum += 100 * int.Parse(number);
					number = sudoku.GetNumber(SudokuOption.GetPosition(0, 1));
					if (number.Length == 1)
						sum += 10 * int.Parse(number);
					number = sudoku.GetNumber(SudokuOption.GetPosition(0, 2));
					if (number.Length == 1)
						sum += int.Parse(number);
				}
				else
					throw new InvalidProgramException("Solved Sudoku incorrectly!");
			}

			return sum.ToString();
		}
	}
}
