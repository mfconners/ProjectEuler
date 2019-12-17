using System.Collections.Generic;
using System.Linq;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem662 : Problem
	{
		public override string CorrectAnswer { get { return "860873428"; } }

		private const long mod = 1000000007;

		private HashSet<(int x, int y)> BuildPossibleSteps(int max_width, int max_height)
		{
			HashSet<(int x, int y)> steps = new HashSet<(int x, int y)>();

			for (int i = 0; i <= max_width; ++i)
			{
				int fib = 1;
				long fib_squared = Fibonacci.GetFibonacci(fib) * Fibonacci.GetFibonacci(fib);
				for (int j = 0; j <= max_height; ++j)
				{
					while (fib_squared - i * i - j * j < 0)
					{
						++fib;
						fib_squared = Fibonacci.GetFibonacci(fib) * Fibonacci.GetFibonacci(fib);
					}

					if (fib_squared - i * i - j * j == 0)
					{
						steps.Add((i, j));
					}
				}
			}

			return steps;
		}

		private string F(int width, int height)
		{
			if (width < height) { int temp = width; width = height; height = temp; }

			HashSet<(int x, int y)> steps = BuildPossibleSteps(width, height);

			List<List<long>> step_counts = new List<List<long>>(width + 1);
			step_counts.Add(new List<long>(Enumerable.Repeat<long>(1, 1)));
			while (step_counts.Count < width + 1)
			{
				step_counts.Add(new List<long>(Enumerable.Repeat<long>(0, (step_counts.Count < height ? step_counts.Count : height) + 1)));
			}

			for ((int x, int y) pos = (0, 0); pos.x <= width; ++pos.x)
			{
				for (pos.y = 0; pos.y <= pos.x && pos.y <= height; ++pos.y)
				{
					foreach ((int x, int y) step in steps)
					{
						if (pos.x >= step.x && pos.y >= step.y)
						{
							(int x, int y) prev_step = (pos.x - step.x, pos.y - step.y);
							if (prev_step.x < prev_step.y) { int temp = prev_step.x; prev_step.x = prev_step.y; prev_step.y = temp; }

							step_counts[pos.x][pos.y] += step_counts[prev_step.x][prev_step.y];

							if (step_counts[pos.x][pos.y] >= long.MaxValue / 2) step_counts[pos.x][pos.y] %= mod;
						}
					}
				}
			}

			return (step_counts[width][height] % mod).ToString();
		}

		protected override string CalculateSolution()
		{
			return F(10000, 10000);
		}
	}
}
