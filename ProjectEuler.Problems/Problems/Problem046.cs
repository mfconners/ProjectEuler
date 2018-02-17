using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem046 : Problem
	{
		public override string CorrectAnswer { get { return "5777"; } }

		protected override string CalculateSolution()
		{
			for (int i = 9; true; i += 2)
			{
				if (!Primes.IsPrime(i))
				{
					for (int j = 1, twicesquared; !Primes.IsPrime(i - (twicesquared = 2 * j * j)); ++j)
					{
						if (twicesquared >= i)
						{
							return i.ToString();
						}
					}
				}
			}
		}
	}
}
