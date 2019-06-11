using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem094 : Problem
	{
		public override string CorrectAnswer { get { return "518408346"; } }

		long max_perimeter = 1000000000;

		protected override string CalculateSolution()
		{
			long perimeter_sum = 0;

			long
				short_side = 1,
				long_side = 2,
				perimeter = 4,
				twice_right_side = 2;

			long diff =
				short_side * short_side
				- long_side * long_side / 4
				- twice_right_side * twice_right_side / 4;

			while (perimeter < max_perimeter)
			{
				while (diff > 0)
				{
					diff -= twice_right_side;
					--diff;
					twice_right_side += 2;
				}

				if (diff == 0)
				{
					perimeter_sum += perimeter;
				}

				diff += 4 * short_side + 3 - long_side;

				short_side += 2;
				long_side += 2;
				perimeter += 6;
			}

			short_side = 3;
			long_side = 2;
			perimeter = 2 * short_side + long_side;
			twice_right_side = 2;
			diff =
				short_side * short_side
				- long_side * long_side / 4
				- twice_right_side * twice_right_side / 4;

			while (perimeter < max_perimeter)
			{
				while (diff > 0)
				{
					diff -= twice_right_side;
					--diff;
					twice_right_side += 2;
				}

				if (diff == 0)
				{
					perimeter_sum += perimeter;
				}

				diff += 4 * short_side + 3 - long_side;

				short_side += 2;
				long_side += 2;
				perimeter += 6;
			}

			return perimeter_sum.ToString();
		}
	}
}
