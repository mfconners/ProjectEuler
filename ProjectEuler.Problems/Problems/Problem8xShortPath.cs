using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	abstract class Problem8xShortPath : Problem
	{
		public virtual bool CanStartAnyLeftToEndAnyRight { get { return false; } }

		protected enum Direction { Down, Up, Left, Right };

		protected List<Direction> _allowedDirections = new List<Direction>();

		private struct Position
		{
			public int x, y;
		}

		static private Position Move(Position start, Direction dir)
		{
			switch (dir)
			{
				case Direction.Down:
					++start.x;
					break;
				case Direction.Up:
					--start.x;
					break;
				case Direction.Right:
					++start.y;
					break;
				case Direction.Left:
					--start.y;
					break;
			}
			return start;
		}

		static private void AddNextStep(SortedDictionary<Int64, List<Position>> priorityQueue, Int64 Cost, Position p)
		{
			if (!priorityQueue.ContainsKey(Cost))
				priorityQueue.Add(Cost, new List<Position>());
			priorityQueue[Cost].Add(p);
		}

		private static readonly char[] separator = { ',' };

		private static readonly char[] newline_separators = { '\r', '\n' };

		private static List<List<Int64>> LoadSquares()
		{
			List<List<Int64>> squares = new List<List<Int64>>();

			string[] matrix_file = Properties.Resources.matrix.Split(newline_separators, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in matrix_file)
			{
				squares.Add(new List<Int64>());
				string[] costs = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				foreach (string cost in costs)
				{
					squares.Last().Add(Convert.ToInt64(cost));
				}
			}

			return squares;
		}

		private static List<List<Int64>> InitSquaresCost(List<List<Int64>> squares)
		{
			List<List<Int64>> squaresCost = new List<List<Int64>>();

			for (int i = 0; i < squares.Count; ++i)
			{
				squaresCost.Add(new List<Int64>());
				for (int j = 0; j < squares[i].Count; ++j)
				{
					squaresCost[i].Add(99999999);
				}
			}

			return squaresCost;
		}

		private SortedDictionary<Int64, List<Position>> InitNextSteps(List<List<Int64>> squares)
		{
			SortedDictionary<Int64, List<Position>> nextSteps = new SortedDictionary<Int64, List<Position>>();

			if (CanStartAnyLeftToEndAnyRight)
			{
				Position p;
				for (p.x = 0, p.y = 0; p.x < squares.Count; ++p.x)
					AddNextStep(nextSteps, squares[p.x][0], p);
			}
			else
			{
				Position p;
				p.x = 0;
				p.y = 0;
				AddNextStep(nextSteps, squares[0][0], p);
			}

			return nextSteps;
		}

		protected override string CalculateSolution()
		{
			List<List<Int64>> squares = LoadSquares();
			List<List<Int64>> squaresCost = InitSquaresCost(squares);
			SortedDictionary<Int64, List<Position>> nextSteps = InitNextSteps(squares);

			while (nextSteps.Count > 0)
			{
				KeyValuePair<Int64, List<Position>> next = nextSteps.First();
				nextSteps.Remove(next.Key);
				for (int i = 0; i < next.Value.Count; ++i)
				{
					Position p = next.Value[i];
					if (p.y == squaresCost[p.x].Count - 1)
						if (p.x == squaresCost[p.x].Count - 1 || CanStartAnyLeftToEndAnyRight)
							return next.Key.ToString();
					if (squaresCost[p.x][p.y] > next.Key)
						squaresCost[p.x][p.y] = next.Key;
				}

				for (int i = 0; i < next.Value.Count; ++i)
					for (int j = 0; j < _allowedDirections.Count; ++j)
					{
						Position oldP = next.Value[i];
						Position newP = Move(oldP, _allowedDirections[j]);
						if (newP.x >= 0 && newP.y >= 0 && newP.x < squares.Count && newP.y < squares[newP.x].Count)
							if (squaresCost[newP.x][newP.y] > next.Key + squares[newP.x][newP.y])
								AddNextStep(nextSteps, next.Key + squares[newP.x][newP.y], newP);
					}
			}

			return string.Empty;
		}
	}
}
