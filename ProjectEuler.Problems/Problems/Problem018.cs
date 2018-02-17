using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem018 : MaximumPathSum
	{
		protected override string Triangle { get { return Properties.Resources.p018; } }

		public override string CorrectAnswer { get { return "1074"; } }
	}
}
