using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem083 : Problem8xShortPath
	{
		public Problem083() { _allowedDirections = GetAllowedDirections(); }

		public override string CorrectAnswer { get { return "425185"; } }

		private static List<Direction> GetAllowedDirections()
		{
			List<Direction> directions = new List<Direction>(4);
			directions.Add(Direction.Down);
			directions.Add(Direction.Right);
			directions.Add(Direction.Up);
			directions.Add(Direction.Left);

			return directions;
		}
	}
}
