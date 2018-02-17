using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEuler.Problems
{
	class Problem067 : MaximumPathSum
	{
		protected override string Triangle { get { return Properties.Resources.triangle; } }

		public override string CorrectAnswer { get { return "7273"; } }
	}
}
