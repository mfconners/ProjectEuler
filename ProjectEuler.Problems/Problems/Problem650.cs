using System.Collections.Generic;
using System.IO;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem650 : Problem
	{
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

			List<Dictionary<long, int>> coefficients = new List<Dictionary<long, int>>(max_n / 2 + 1);
			List<Dictionary<long, int>> prev_coefficients = new List<Dictionary<long, int>>(max_n / 2 + 1);
			while (coefficients.Count < max_n / 2 + 1)
			{
				prev_coefficients.Add(new Dictionary<long, int>());
				coefficients.Add(new Dictionary<long, int>());
			}

			Dictionary<long, int> D_factors = new Dictionary<long, int>();

			for (int n = 1; n <= max_n; ++n)
			{
				List<Dictionary<long, int>> temp = prev_coefficients;
				prev_coefficients = coefficients;
				coefficients = temp;

				D_factors.Clear();
				for (int k = 1; k <= n / 2; ++k)
				{
					coefficients[k].Clear();
					if (2 * k < n)
					{
						long remainder_large = 1, remainder_small = 1;

						foreach (var prime_factor_large in prev_coefficients[k])
						{
							if (prev_coefficients[k - 1].ContainsKey(prime_factor_large.Key))
							{
								if (prev_coefficients[k - 1][prime_factor_large.Key] == prime_factor_large.Value)
								{
									coefficients[k].Add(prime_factor_large.Key, prime_factor_large.Value);

									if (D_factors.ContainsKey(prime_factor_large.Key))
										D_factors[prime_factor_large.Key] += 2 * prime_factor_large.Value;
									else
										D_factors.Add(prime_factor_large.Key, 2 * prime_factor_large.Value);

									prev_coefficients[k - 1].Remove(prime_factor_large.Key);
								}
								else if (prev_coefficients[k - 1][prime_factor_large.Key] > prime_factor_large.Value)
								{
									coefficients[k].Add(prime_factor_large.Key, prime_factor_large.Value);

									if (D_factors.ContainsKey(prime_factor_large.Key))
										D_factors[prime_factor_large.Key] += 2 * prime_factor_large.Value;
									else
										D_factors.Add(prime_factor_large.Key, 2 * prime_factor_large.Value);

									prev_coefficients[k - 1][prime_factor_large.Key] -= prime_factor_large.Value;
								}
								else
								{
									coefficients[k].Add(prime_factor_large.Key, prev_coefficients[k - 1][prime_factor_large.Key]);

									if (D_factors.ContainsKey(prime_factor_large.Key))
										D_factors[prime_factor_large.Key] += 2 * prev_coefficients[k - 1][prime_factor_large.Key];
									else
										D_factors.Add(prime_factor_large.Key, 2 * prev_coefficients[k - 1][prime_factor_large.Key]);

									for (prev_coefficients[k - 1][prime_factor_large.Key] -= prime_factor_large.Value; prev_coefficients[k - 1][prime_factor_large.Key] < 0; ++prev_coefficients[k - 1][prime_factor_large.Key])
									{
										remainder_large *= prime_factor_large.Key;
									}
									prev_coefficients[k - 1].Remove(prime_factor_large.Key);
								}
							}
							else
							{
								for (int power = 0; power < prime_factor_large.Value; ++power)
									remainder_large *= prime_factor_large.Key;
							}
						}

						foreach (var prime_factor_small in prev_coefficients[k - 1])
						{
							for (int power = 0; power < prime_factor_small.Value; ++power)
								remainder_small *= prime_factor_small.Key;
						}

						long remainder = remainder_small + remainder_large;

						for (int p = 0, prime = Primes.GetPrime(0); remainder > 1; prime = Primes.GetPrime(++p))
						{
							if (remainder % prime == 0)
							{
								if (!coefficients[k].ContainsKey(prime))
								{
									coefficients[k].Add(prime, 1);
									remainder /= prime;

									if (D_factors.ContainsKey(prime))
										D_factors[prime] += 2;
									else
										D_factors.Add(prime, 2);
								}
								while (remainder % prime == 0)
								{
									++coefficients[k][prime];
									remainder /= prime;
									D_factors[prime] += 2;
								}
							}
							if (prime * prime > remainder && remainder > 1)
							{
								if (!coefficients[k].ContainsKey(remainder))
								{
									coefficients[k].Add(remainder, 1);
									if (D_factors.ContainsKey(remainder))
										D_factors[remainder] += 2;
									else
										D_factors.Add(remainder, 2);
								}
								else
								{
									++coefficients[k][remainder];
									D_factors[remainder] += 2;
								}
								remainder = 1;

								break;
							}
						}
					}
					else
					{
						foreach (var prime_factor in prev_coefficients[k - 1])
						{
							coefficients[k].Add(prime_factor.Key, prime_factor.Value);
							if (D_factors.ContainsKey(prime_factor.Key))
								D_factors[prime_factor.Key] += prime_factor.Value;
							else
								D_factors.Add(prime_factor.Key, prime_factor.Value);
						}

						if (coefficients[k].ContainsKey(2))
							++coefficients[k][2];
						else
							coefficients[k].Add(2, 1);

						if (D_factors.ContainsKey(2))
							++D_factors[2];
						else
							D_factors.Add(2, 1);
					}
				}

				long D = 1;
				foreach (var factor in D_factors)
				{
					long prime_power = 1;
					long power_sum = 1;
					for (int power = 0; power < factor.Value; ++power)
					{
						prime_power *= factor.Key;
						prime_power %= modder;
						power_sum += prime_power;
						power_sum %= modder;
					}
					D *= power_sum;
					D %= modder;
				}

				sum += D;
				sum %= modder;
			}

			return sum.ToString();
		}
	}
}
