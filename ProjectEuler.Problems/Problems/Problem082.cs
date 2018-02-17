using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem082 : Problem8xShortPath
	{
		public Problem082() { _allowedDirections = GetAllowedDirections(); }

		public override bool CanStartAnyLeftToEndAnyRight { get { return true; } }

		public override string CorrectAnswer { get { return "260324"; } }

		private static List<Direction> GetAllowedDirections()
		{
			List<Direction> directions = new List<Direction>(2);
			directions.Add(Direction.Down);
			directions.Add(Direction.Right);
			directions.Add(Direction.Up);

			return directions;
		}
	}
}
