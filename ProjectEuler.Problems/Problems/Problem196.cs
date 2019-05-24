using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem196 : Problem
	{
		// Slow: >8 minutes
		#region Units of work
		private class WorkUnit
		{
			private Int64 _min, _max, _row;
			public Int64 Min
			{
				get
				{
					return _min;
				}
				private set
				{
					if (value % 2 == 0)
					{
						_min = value + 1;
					}
					else
					{
						_min = value;
					}
				}
			}
			public Int64 Max
			{
				get { return _max; }
				private set
				{
					if (value % 2 == 0)
					{
						_max = value - 1;
					}
					else
					{
						_max = value;
					}
				}
			}
			public Int64 Row { get { return _row; } }
			public WorkUnit(Int64 row)
			{
				_row = row;
				_max = row * (row + 1) / 2;
				_min = _max - row + 1;
				Max = _max;
				Min = _min;
			}
			public WorkUnit(WorkUnit unit, Int64 chunkSize = 0, Int64 pieces = 0, Int64 smallestChunk = 1)
			{
				if (pieces <= 0)
					pieces = Environment.ProcessorCount;

				_row = unit.Row;
				_min = unit._min;

				if (chunkSize <= 0)
				{
					chunkSize = (unit._max - unit._min) / pieces;
					if (chunkSize < smallestChunk)
						chunkSize = smallestChunk;
				}
				if (unit._min + chunkSize >= unit._max)
					_max = unit._max;
				else
					_max = _min + chunkSize;

				unit.Min = _max + 1;
				Min = _min;
				Max = _max;
			}
			public override string ToString()
			{
				return Convert.ToString(Min) + " to " + Convert.ToString(Max);
			}
		}

		private static long _sumTotal = 0;
		private static SemaphoreSlim _manageWork = new SemaphoreSlim(1);
		private static Queue<WorkUnit> _workUnits = new Queue<WorkUnit>();
		private static SemaphoreSlim _resultsLock = new SemaphoreSlim(Environment.ProcessorCount);
		#endregion

		public override string CorrectAnswer { get { return "322303240771079935"; } }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Dictionary<Int64, bool> InitIsPrime(long min, long max)
		{
			Dictionary<Int64, bool> isPrime = new Dictionary<long, bool>();

			for (Int64 testTriplet = min; testTriplet <= max; testTriplet += 2)
			{
				if (!isPrime.ContainsKey(testTriplet))
				{
					isPrime.Add(testTriplet, Primes.IsPrime(testTriplet, false));
				}
			}

			return isPrime;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void TestEvenRowNeighbors(Dictionary<Int64, HashSet<Int64>> neighbors, long testTriplet, long row)
		{
			Int64 maxRow = row * (row + 1) / 2;
			Int64 minRow = maxRow - row + 1;
			Int64 maxLow1 = minRow - 1;
			Int64 minLow1 = minRow - row + 1;
			Int64 maxLow2 = minLow1 - 1;
			Int64 minLow2 = minLow1 - row + 2;
			Int64 minHigh1 = maxRow + 1;
			Int64 maxHigh1 = minHigh1 + row;
			Int64 minHigh2 = maxHigh1 + 1;
			Int64 maxHigh2 = minHigh2 + row + 1;

			Int64 firstNeighbor, secondNeighbor;
			#region (testTriplet - row + 2) --> (testTriplet - 2 * row + 4) and (testTriplet + 2)
			firstNeighbor = testTriplet - row + 2;
			if (firstNeighbor <= maxLow1 && firstNeighbor >= minLow1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet + 2;
				if (secondNeighbor <= maxRow && secondNeighbor >= minRow)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet - 2 * row + 4;
				if (secondNeighbor <= maxLow2 && secondNeighbor >= minLow2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
			#region (testTriplet - row) --> (testTriplet - 2 * row + 2) and (testTriplet - 2)
			firstNeighbor = testTriplet - row;
			if (firstNeighbor <= maxLow1 && firstNeighbor >= minLow1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet - 2 * row + 2;
				if (secondNeighbor <= maxLow2 && secondNeighbor >= minLow2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet - 2;
				if (secondNeighbor <= maxRow && secondNeighbor >= minRow)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
			#region (testTriplet + row) --> (testTriplet + 2 * row) and (testTriplet + 2 * row + 2)
			firstNeighbor = testTriplet + row;
			if (firstNeighbor <= maxHigh1 && firstNeighbor >= minHigh1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet + 2 * row + 2;
				if (secondNeighbor <= maxHigh2 && secondNeighbor >= minHigh2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet + 2 * row;
				if (secondNeighbor <= maxHigh2 && secondNeighbor > minHigh2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
		}

		private static void TestOddRowNeighbors(Dictionary<Int64, HashSet<Int64>> neighbors, long testTriplet, long row)
		{
			Int64 maxRow = row * (row + 1) / 2;
			Int64 minRow = maxRow - row + 1;
			Int64 maxLow1 = minRow - 1;
			Int64 minLow1 = minRow - row + 1;
			Int64 maxLow2 = minLow1 - 1;
			Int64 minLow2 = minLow1 - row + 2;
			Int64 minHigh1 = maxRow + 1;
			Int64 maxHigh1 = minHigh1 + row;
			Int64 minHigh2 = maxHigh1 + 1;
			Int64 maxHigh2 = minHigh2 + row + 1;

			Int64 firstNeighbor, secondNeighbor;
			#region (testTriplet + row + 1) --> (testTriplet + 2 * row + 2) and (testTriplet + 2)
			firstNeighbor = testTriplet + row + 1;
			if (firstNeighbor <= maxHigh1 && firstNeighbor >= minHigh1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet + 2 * row + 2;
				if (secondNeighbor <= maxHigh2 && secondNeighbor >= minHigh2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet + 2;
				if (secondNeighbor <= maxRow && secondNeighbor >= minRow)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
			#region (testTriplet + row - 1) --> (testTriplet + 2 * row) and (testTriplet - 2)
			firstNeighbor = testTriplet + row - 1;
			if (firstNeighbor <= maxHigh1 && firstNeighbor >= minHigh1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet + 2 * row;
				if (secondNeighbor <= maxHigh2 && secondNeighbor >= minHigh2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet - 2;
				if (secondNeighbor <= maxRow && secondNeighbor >= minRow)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
			#region (testTriplet - row + 1) --> (testTriplet - 2 * row + 2) and (testTriplet - 2 * row + 4)
			firstNeighbor = testTriplet - row + 1;
			if (firstNeighbor <= maxLow1 && firstNeighbor >= minLow1)
			{
				neighbors[testTriplet].Add(firstNeighbor);
				// TODO Garbage Collection: Allocating at a high rate?
				neighbors.Add(firstNeighbor, new HashSet<Int64>());
				secondNeighbor = testTriplet - 2 * row + 2;
				if (secondNeighbor <= maxLow2 && secondNeighbor >= minLow2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
				secondNeighbor = testTriplet - 2 * row + 4;
				if (secondNeighbor <= maxLow2 && secondNeighbor >= minLow2)
				{
					neighbors[firstNeighbor].Add(secondNeighbor);
				}
			}
			#endregion
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void FillNeighborhood(Dictionary<Int64, HashSet<Int64>> neighbors, long testTriplet, long row)
		{
			neighbors.Clear();
			// TODO Garbage Collection: Allocating at a high rate?
			neighbors.Add(testTriplet, new HashSet<long>());

			if (row % 2 == 0)
			{
				TestEvenRowNeighbors(neighbors, testTriplet, row);
			}
			else
			{
				TestOddRowNeighbors(neighbors, testTriplet, row);
			}
		}

		public override void AssistCalculateSolution()
		{
			SpinWait spinWait = new SpinWait();

			if (_resultsLock.Wait(0))
			{
				while (_workUnits.Count > 0)
				{
					WorkUnit unit = null;
					_manageWork.Wait();
					if (_workUnits.Count > 0)
					{
						unit = _workUnits.Dequeue();
					}
					_manageWork.Release();

					if (unit != null)
					{
						Int64 sum = 0;

						Queue<Int64> possibles = new Queue<Int64>();
						Queue<Int64> priorityPossibles = new Queue<Int64>();
						Dictionary<Int64, HashSet<Int64>> neighbors = new Dictionary<Int64, HashSet<Int64>>();
						Dictionary<Int64, bool> isPrime = InitIsPrime(unit.Min, unit.Max);

						for (Int64 testTriplet = unit.Min; testTriplet <= unit.Max; testTriplet += 2)
						{
							if (!isPrime.ContainsKey(testTriplet) || isPrime[testTriplet])
							{
								possibles.Clear();
								possibles.Enqueue(testTriplet);
								priorityPossibles.Clear();

								FillNeighborhood(neighbors, testTriplet, unit.Row);

								#region Check the possible primes.
								int primes = 0;
								while (primes < 3 && (possibles.Count > 0 || priorityPossibles.Count > 0))
								{
									long primeTest;
									if (priorityPossibles.Count > 0)
									{
										primeTest = priorityPossibles.Dequeue();
									}
									else
									{
										primeTest = possibles.Dequeue();
									}

									if (!isPrime.ContainsKey(primeTest))
									{
										isPrime.Add(primeTest, Primes.IsPrime(primeTest, false));
									}

									if (isPrime[primeTest])
									{
										++primes;
										if (primes < 3 && neighbors.ContainsKey(primeTest))
										{
											foreach (long neighbor in neighbors[primeTest])
											{
												if (!isPrime.ContainsKey(neighbor))
												{
													possibles.Enqueue(neighbor);
												}
												else if (isPrime[neighbor])
												{
													priorityPossibles.Enqueue(neighbor);
												}
											}
										}
									}
								}
								#endregion

								if (primes >= 3)
								{
									sum += testTriplet;
								}
							}
						}

						while (true)
						{
							Int64 oldSum1 = _sumTotal;
							Int64 oldSum2 = Interlocked.CompareExchange(ref _sumTotal, oldSum1 + sum, oldSum1);
							if (oldSum1 == oldSum2)
								break;
							spinWait.SpinOnce();
						}
					}
				}
				_resultsLock.Release();
			}
		}

		protected override string CalculateSolution()
		{
			//return (SumRow(5678027) + SumRow(7208785)).ToString();
			#region Create Work
			_manageWork.Wait();
			if (_sumTotal == 0 && _workUnits.Count == 0)
			{
				WorkUnit remainder7208785 = new WorkUnit(7208785);
				_workUnits.Enqueue(new WorkUnit(remainder7208785));
				WorkUnit remainder5678027 = new WorkUnit(5678027);
				while (remainder5678027.Min <= remainder5678027.Max)
				{
					WorkUnit unit = new WorkUnit(remainder5678027, 0, 32, 2048);
					_workUnits.Enqueue(unit);
				}

				while (remainder7208785.Min <= remainder7208785.Max)
				{
					WorkUnit unit = new WorkUnit(remainder7208785, 0, 32, 128);
					_workUnits.Enqueue(unit);
				}
			}
			_manageWork.Release();
			#endregion

			#region Do Work
			AssistCalculateSolution();
			#endregion

			#region Wait for unfinished work from other processes
			for (int i = 0; i < Environment.ProcessorCount; ++i)
				_resultsLock.Wait();
			#endregion

			return _sumTotal.ToString();
		}
	}
}
