using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectEuler.Problems
{
	class Problem062 : Problem
	{
		public override string CorrectAnswer { get { return "127035954683"; } }

		private const int count_search = 5;

		protected override string CalculateSolution()
		{
			Dictionary<string, long> CubePermutationMinCube = new Dictionary<string, long>();
			Dictionary<string, int> CubePermutationCount = new Dictionary<string, int>();

			for (long cuberoot = 1; ; ++cuberoot)
			{
				long cube = cuberoot * cuberoot * cuberoot;
				char[] cubechars = cube.ToString().ToCharArray();
				Array.Sort(cubechars);
				string uniquekey = new string(cubechars);

				if (CubePermutationCount.ContainsKey(uniquekey))
				{
					++CubePermutationCount[uniquekey];
					if (CubePermutationCount[uniquekey] >= count_search)
					{
						return CubePermutationMinCube[uniquekey].ToString();
					}
				}
				else
				{
					CubePermutationMinCube.Add(uniquekey, cube);
					CubePermutationCount.Add(uniquekey, 1);
				}
			}
		}
	}
}
