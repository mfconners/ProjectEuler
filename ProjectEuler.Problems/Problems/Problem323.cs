using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem323 : Problem
	{
		public override string CorrectAnswer { get { return "6.3551758451"; } }

		private double Multiplier(int i, int j)
		{
			double multiplier = 1.0;

			for (int k = 1; k <= 32; ++k)
			{
				int mod = (k <= 32 - j ? 1 : 0) + (k <= i - j ? -1 : 0) + (k <= 32 - i ? -1 : 0);


				if (k > 1)
				{
					if (mod > 0)
						multiplier *= k;
					else if (mod < 0)
						multiplier /= k;
				}

				if (k <= 32 - j)
					multiplier /= 2.0;
			}

			return multiplier;
		}

		protected override string CalculateSolution()
		{
			List<double> probability = new List<double>(33);
			probability.Add(1.0);
			while (probability.Count < 33)
			{
				probability.AddRange(Enumerable.Repeat<double>(0.0, 32));
			}
			List<double> new_probability = new List<double>(probability);

			int steps = 0;
			double expected_value = 0.0;
			for (double diff_expected_value = 1.0; diff_expected_value > 0.00000000001; diff_expected_value = expected_value - diff_expected_value)
			{
				diff_expected_value = expected_value;

				for (int i = 0; i < 33; ++i)
				{
					new_probability[i] = 0.0;
					for (int j = 0; j <= i; ++j)
					{
						if (probability[j] != 0.0)
							new_probability[i] += Multiplier(i, j) * probability[j];
					}
				}

				++steps;
				expected_value += steps * new_probability[32];
				new_probability[32] = 0.0;

				List<double> temp = new_probability;
				new_probability = probability;
				probability = temp;
			}

			return expected_value.ToString("0.0000000000");
		}
	}
}
