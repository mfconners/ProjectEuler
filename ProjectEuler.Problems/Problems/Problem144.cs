using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ProjectEuler.Problems
{
	class Problem144 : Problem
	{
		public override string CorrectAnswer { get { return "354"; } }

		readonly double cross_at_y = Math.Sqrt(100 - 4 * 0.1 * 0.1);
		readonly double hole_left = -0.01, hole_right = 0.01;

		protected override string CalculateSolution()
		{
			Vector position = new Vector(0.0, 10.1);
			Vector vector = new Vector(1.4 - position.X, -9.6 - position.Y);
			vector /= vector.Length;

			int bounces = 0;

			for (double x_at_y10 = 9999.9;
				x_at_y10 < hole_left || x_at_y10 > hole_right;
				x_at_y10 = position.X + vector.X * (cross_at_y - position.Y) / vector.Y, ++bounces)
			{
				double a =
					4 * Math.Pow(vector.X, 2)
					+ Math.Pow(vector.Y, 2);
				double b =
					8 * vector.X * position.X
					+ 2 * vector.Y * position.Y;
				double c =
					4 * Math.Pow(position.X, 2)
					+ Math.Pow(position.Y, 2)
					- 100;

				double magnitude = -b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c);
				magnitude /= 2 * a;

				position = position + magnitude * vector;


				Vector normal = new Vector(-4 * position.X, -position.Y);
				normal /= normal.Length;

				vector = vector - 2 * (vector * normal) * normal;
				vector /= vector.Length;
			}



			return bounces.ToString();
		}
	}
}
