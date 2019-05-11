using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem085 : Problem
	{
		public override string CorrectAnswer { get { return "2772"; } }

		private long CountRectangles(long height, long width)
		{
			long count = 0;

			for (long h = height; h > 0; --h)
			{
				for (long w = width; w > 0; --w)
				{
					count += h * w;
				}
			}

			return count;
		}

		private const long match_target = 2000000;

		protected override string CalculateSolution()
		{
			long area = 0;
			long closest_diff = match_target;

			for (long height = 1, width = match_target, max_jump = 0x1 << 30; height < width; ++height)
			{
				while (max_jump > width) max_jump /= 2;
				long diff = Math.Abs(CountRectangles(height, width) - match_target);

				for (long jump = max_jump; jump > 0; jump /= 2)
				{
					long width_down = width - jump;
					if (width_down < height)
						width_down = height - 1;
					long diff_down = Math.Abs(CountRectangles(height, width_down) - match_target);

					long width_up = width + jump;
					long diff_up = Math.Abs(CountRectangles(height, width_up) - match_target);

					if (diff_down < diff && diff_down <= diff_up)
					{
						width = width_down;
						diff = diff_down;
					}
					else if (diff_up < diff && diff_up <= diff_down)
					{
						width = width_up;
						diff = diff_up;
					}
				}

				if (diff<closest_diff)
				{
					area = height * width;
					closest_diff = diff;
				}
			}

			return area.ToString();
		}
	}
}
