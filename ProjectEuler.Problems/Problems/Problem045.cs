using System;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem045 : Problem
	{
		public override string CorrectAnswer { get { return "1533776805"; } }

		protected override string CalculateSolution()
		{
			Int64 triangle_index = 285 + 1, triangle = triangle_index * (triangle_index + 1) / 2;
			Int64 pentagon_index = 165, pentagon = pentagon_index * (3 * pentagon_index - 1) / 2;
			Int64 hexagon_index = 143, hexagon = hexagon_index * (2 * hexagon_index - 1);

			while (triangle != pentagon || triangle != hexagon)
			{
				while (triangle < pentagon || triangle < hexagon)
				{
					++triangle_index;
					triangle = triangle_index * (triangle_index + 1) / 2;
				}
				while (pentagon < triangle || pentagon < hexagon)
				{
					++pentagon_index;
					pentagon = pentagon_index * (3 * pentagon_index - 1) / 2;
				}
				while (hexagon < triangle || hexagon < pentagon)
				{
					++hexagon_index;
					hexagon = hexagon_index * (2 * hexagon_index - 1);
				}
			}

			return triangle.ToString();
		}
	}
}
