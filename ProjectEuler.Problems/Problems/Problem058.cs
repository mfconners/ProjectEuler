using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem058 : Problem
	{
		public override string CorrectAnswer { get { return "26241"; } }

		protected override string CalculateSolution()
		{
			long prime_count = 3, total_count = 5;

			long diagonal = 9;
			long side = 2;

			while (10 * prime_count > total_count)
			{
				side += 2;
				total_count += 4;

				for (int i = 0; i < 4; ++i)
				{
					diagonal += side;
					if (i < 3 && Primes.IsPrime(diagonal, false))
						++prime_count;
				}
			}

			return (side + 1).ToString();
		}
	}
}
