using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem650 : Problem
	{
		// Slow: >1 hour
		public override string CorrectAnswer { get { return "538319652"; } }

		//const int max_n = 5;
		//const int max_n = 10;
		//const int max_n = 100;
		const int max_n = 20000;

		const long modder = 1000000007;
		//const long modder = long.MaxValue;

		protected override string CalculateSolution()
		{
			long sum = 0;

			List<int> D_factors = new List<int>();
			List<int> D_multiplier = new List<int>();

			Dictionary<int, int> factored_multiplier = new Dictionary<int, int>();
			Dictionary<int, int> factored_divisor = new Dictionary<int, int>();

			for (int n = 1; n <= max_n; ++n)
			{
				{
					Dictionary<int, int> temp = factored_divisor;
					factored_divisor = factored_multiplier;
					factored_multiplier = temp;
					factored_multiplier.Clear();
				}

				if (Primes.IsPrime(n))
				{
					factored_multiplier.Add(Primes.IndexOfPrimeAtMost(n), 1);
				}
				else
				{
					for (int p = 0, prime = Primes.GetPrime(p), remainder = n; remainder > 1; prime = Primes.GetPrime(++p))
					{
						if (remainder % prime == 0)
						{
							factored_multiplier.Add(p, 1);
							remainder /= prime;
							while (remainder % prime == 0)
							{
								factored_multiplier[p]++;
								remainder /= prime;
							}
						}
					}
				}

				foreach (var mult_factor in factored_multiplier)
				{
					if (mult_factor.Key >= D_multiplier.Count)
						D_multiplier.Add(mult_factor.Value * (n - 1));
					else
						D_multiplier[mult_factor.Key] += mult_factor.Value * (n - 1);
				}

				foreach (var div_factor in factored_divisor)
				{
					D_multiplier[div_factor.Key] -= div_factor.Value * (n - 1);
				}

				while (D_factors.Count < D_multiplier.Count) D_factors.Add(0);

				for (int m = 0; m < D_multiplier.Count; ++m)
				{
					D_factors[m] += D_multiplier[m];
				}

				long D = 1;
				for (int f = 0; f < D_factors.Count; ++f)
				{
					if (D_factors[f] < 0) throw new System.Exception();
					if (D_factors[f] > 0)
					{
						int prime = Primes.GetPrime(f);
						int max_power = D_factors[f];
						long prime_power = 1;
						long power_sum = 1;
						for (int power = 0; power < max_power; ++power)
						{
							prime_power *= prime;
							prime_power %= modder;
							power_sum += prime_power;
							power_sum %= modder;
						}
						D *= power_sum;
						D %= modder;
					}
				}

				sum += D;
				sum %= modder;
			}

			return sum.ToString();
		}
	}
}
