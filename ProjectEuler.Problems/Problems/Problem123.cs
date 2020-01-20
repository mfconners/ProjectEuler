using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem123 : Problem
	{
		public override string CorrectAnswer { get { return "21035"; } }

		const long min_remainder = 10000000000;
		//const long min_remainder = 1000000000;

		protected override string CalculateSolution()
		{
			for (int p = 0; true; ++p)
			{
				long prime = Primes.GetPrime(p);
				long prime_sq = prime * prime;
				if (prime_sq < min_remainder) continue;

				long prime_minus_1 = prime - 1;
				long prime_plus_1 = prime + 1;
				long remainder_minus = 1;
				long remainder_plus = 1;
				for (int n = 0; n <= p; ++n)
				{
					remainder_minus *= prime_minus_1;
					if (remainder_minus >= prime_sq)
					{
						remainder_minus %= prime_sq;
					}

					remainder_plus *= prime_plus_1;
					if (remainder_plus >= prime_sq)
					{
						remainder_plus %= prime_sq;
					}
				}

				long remainder = (remainder_minus + remainder_plus) % prime_sq;
				if (remainder > min_remainder)
				{
					return (p + 1).ToString();
				}
			}
		}
	}
}
