using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem081 : Problem8xShortPath
	{
		public Problem081()
		{
			_allowedDirections = GetAllowedDirections();
		}

		public override string CorrectAnswer { get { return "427337"; } }

		private static List<Direction> GetAllowedDirections()
		{
			List<Direction> directions = new List<Direction>(2);
			directions.Add(Direction.Down);
			directions.Add(Direction.Right);

			return directions;
		}
	}
}
