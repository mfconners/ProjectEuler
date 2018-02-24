using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ProjectEuler.MathExtensions;

namespace ProjectEuler.Problems
{
	class Problem619 : Problem
	{
		//public override string CorrectAnswer { get { return "???"; } }

		const int c_min = 1000000, c_max = 1234567;
		const int modulo_base = 1000000007;

		private class DistinctPrimeComposite
		{
			[ThreadStatic]
			private static Stack<int> workspace = new Stack<int>();

			public static DistinctPrimeComposite RootFactor { get { return root_factor; } }

			private static DistinctPrimeComposite root_factor = new DistinctPrimeComposite(1, null);
			private Dictionary<int, WeakReference<DistinctPrimeComposite>> larger_prime_factors = new Dictionary<int, WeakReference<DistinctPrimeComposite>>();
			private Dictionary<DistinctPrimeComposite, DistinctPrimeComposite> multiplier_cache = new Dictionary<DistinctPrimeComposite, DistinctPrimeComposite>();
			private DistinctPrimeComposite _smaller_parent = null;
			private int _max_prime_factor;
			public int max_prime_factor { get { return _max_prime_factor; } }

			private DistinctPrimeComposite(int prime_factor, DistinctPrimeComposite parent)
			{
				_max_prime_factor = prime_factor;
				_smaller_parent = parent;
			}

			public override string ToString()
			{
				if (max_prime_factor <= 1)
					return max_prime_factor.ToString();

				StringBuilder builder = new StringBuilder();
				builder.Append(max_prime_factor);

				DistinctPrimeComposite dpc = this._smaller_parent;
				while (dpc.max_prime_factor > 1)
				{
					builder.Append(" * ");
					builder.Append(dpc.max_prime_factor);
				}

				return builder.ToString();
			}

			public static DistinctPrimeComposite GetDistinctPrimeComposite(int composite)
			{
				DistinctPrimeComposite dpc = root_factor;
				for (int p = 0, prime = Primes.GetPrime(0); composite > 1; prime = Primes.GetPrime(++p))
				{
					int prime_squared = prime * prime;
					if (prime_squared > composite)
					{
						prime = composite;
					}
					else
					{
						while (composite % prime_squared == 0)
						{
							composite /= prime_squared;
						}
					}

					if (composite % prime == 0)
					{
						DistinctPrimeComposite new_dpc;
						if (dpc.larger_prime_factors.ContainsKey(prime))
						{
							if (!dpc.larger_prime_factors[prime].TryGetTarget(out new_dpc))
							{
								new_dpc = new DistinctPrimeComposite(prime, dpc);
								dpc.larger_prime_factors[prime].SetTarget(new_dpc);
							}
						}
						else
						{
							new_dpc = new DistinctPrimeComposite(prime, dpc);
							dpc.larger_prime_factors.Add(prime, new WeakReference<DistinctPrimeComposite>(new_dpc));
						}
						dpc = new_dpc;
						composite /= prime;
					}

				}
				return dpc;
			}

			public static DistinctPrimeComposite Multiply(DistinctPrimeComposite m1, DistinctPrimeComposite m2)
			{
				if (m1 == null)
				{
					if (m2 == null)
					{
						return root_factor;
					}
					else
					{
						return m2;
					}
				}
				else if (m2 == null)
				{
					return m1;
				}

				if (m1.multiplier_cache.ContainsKey(m2))
				{
					return m1.multiplier_cache[m2];
				}
				else if (m2.multiplier_cache.ContainsKey(m1))
				{
					return m2.multiplier_cache[m1];
				}

				DistinctPrimeComposite original_m1 = m1, original_m2 = m2;

				while (m1 != root_factor && m2 != root_factor)
				{
					if (m1.max_prime_factor == m2.max_prime_factor)
					{
						m1 = m1._smaller_parent;
						m2 = m2._smaller_parent;
					}
					else if (m1.max_prime_factor > m2.max_prime_factor)
					{
						workspace.Push(m1.max_prime_factor);
						m1 = m1._smaller_parent;
					}
					else
					{
						workspace.Push(m2.max_prime_factor);
						m2 = m2._smaller_parent;
					}
				}

				DistinctPrimeComposite dpc = m1;
				if (dpc == root_factor)
					dpc = m2;

				while (workspace.Count > 0)
				{
					int factor = workspace.Pop();
					if (dpc.larger_prime_factors.ContainsKey(factor))
					{
						DistinctPrimeComposite new_dpc;
						if (!dpc.larger_prime_factors[factor].TryGetTarget(out new_dpc))
						{
							new_dpc = new DistinctPrimeComposite(factor, dpc);
							dpc.larger_prime_factors[factor].SetTarget(new_dpc);
						}
						dpc = new_dpc;
					}
					else
					{
						DistinctPrimeComposite new_dpc = new DistinctPrimeComposite(factor, dpc);
						dpc.larger_prime_factors.Add(factor, new WeakReference<DistinctPrimeComposite>(new_dpc));
						dpc = new_dpc;
					}
				}

				if (original_m1.max_prime_factor > original_m2.max_prime_factor)
				{
					original_m1.multiplier_cache.Add(original_m2, dpc);
				}

				return dpc;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int CountCompositeFactors(DistinctPrimeComposite factor1, DistinctPrimeComposite factor2, Dictionary<DistinctPrimeComposite, long> counts)
		{
			if (counts == null)
				throw new ArgumentNullException("counts");
			if (factor1 == null)
				throw new ArgumentNullException("factor1");
			if (factor2 == null)
				throw new ArgumentNullException("factor2");

			if (!counts.ContainsKey(DistinctPrimeComposite.RootFactor))
			{
				counts.Add(DistinctPrimeComposite.RootFactor, 1);
			}

			int max_prime = 0;

			DistinctPrimeComposite dpc_multiple = DistinctPrimeComposite.Multiply(factor1, factor2);

			if (dpc_multiple.max_prime_factor > max_prime)
				max_prime = dpc_multiple.max_prime_factor;
			if (!counts.ContainsKey(dpc_multiple))
				counts.Add(dpc_multiple, 1);
			else
				++counts[dpc_multiple];

			if (factor1.max_prime_factor > max_prime)
				max_prime = factor1.max_prime_factor;
			if (!counts.ContainsKey(factor1))
				counts.Add(factor1, 1);
			else
				++counts[factor1];

			if (factor2.max_prime_factor > max_prime)
				max_prime = factor2.max_prime_factor;
			if (!counts.ContainsKey(factor2))
				counts.Add(factor2, 1);
			else
				++counts[factor2];

			return max_prime;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int CountCompositeCounts(Dictionary<DistinctPrimeComposite, long> old_counts1, Dictionary<DistinctPrimeComposite, long> old_counts2, Dictionary<DistinctPrimeComposite, long> counts, int max_prime_to_keep = int.MaxValue)
		{
			if (counts == null)
				throw new ArgumentNullException("counts");
			if (old_counts1 == null)
				throw new ArgumentNullException("old_counts1");
			if (old_counts2 == null)
				throw new ArgumentNullException("old_counts2");

			int max_prime = 0;

			foreach (DistinctPrimeComposite old_factor1 in old_counts1.Keys)
			{
				foreach (DistinctPrimeComposite old_factor2 in old_counts2.Keys)
				{
					DistinctPrimeComposite new_factor = DistinctPrimeComposite.Multiply(old_factor1, old_factor2);
					if (new_factor.max_prime_factor <= max_prime_to_keep)
					{
						if (new_factor.max_prime_factor > max_prime)
							max_prime = new_factor.max_prime_factor;

						if (counts.ContainsKey(new_factor))
						{
							counts[new_factor] += old_counts1[old_factor1] * old_counts2[old_factor2];
							counts[new_factor] %= modulo_base;
						}
						else
						{
							counts.Add(new_factor, (old_counts1[old_factor1] * old_counts2[old_factor2]) % modulo_base);
						}
					}
				}
			}

			return max_prime;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int CountCompositeFactorAndCount(DistinctPrimeComposite factor, Dictionary<DistinctPrimeComposite, long> old_counts, Dictionary<DistinctPrimeComposite, long> counts, int max_prime_to_keep = int.MaxValue)
		{
			if (counts == null)
				throw new ArgumentNullException("counts");
			if (factor == null)
				throw new ArgumentNullException("factor");
			if (old_counts == null)
				throw new ArgumentNullException("old_counts2");

			int max_prime = 0;

			foreach (DistinctPrimeComposite old_factor in old_counts.Keys)
			{
				if (old_factor.max_prime_factor <= max_prime_to_keep)
				{
					if (old_factor.max_prime_factor > max_prime)
						max_prime = old_factor.max_prime_factor;

					if (!counts.ContainsKey(old_factor))
					{
						counts.Add(old_factor, old_counts[old_factor]);
					}
					else
					{
						counts[old_factor] += old_counts[old_factor];
						counts[old_factor] %= modulo_base;
					}
				}

				DistinctPrimeComposite new_factor = DistinctPrimeComposite.Multiply(factor, old_factor);
				if (new_factor.max_prime_factor <= max_prime_to_keep)
				{
					if (new_factor.max_prime_factor > max_prime)
						max_prime = new_factor.max_prime_factor;

					if (!counts.ContainsKey(new_factor))
					{
						counts.Add(new_factor, old_counts[old_factor]);
					}
					else
					{
						counts[new_factor] += old_counts[old_factor];
						counts[new_factor] %= modulo_base;
					}
				}
			}

			return max_prime;
		}

		private int DropMaxPrime(Dictionary<DistinctPrimeComposite, long> old_counts, Dictionary<DistinctPrimeComposite, long> counts, int max_prime_to_keep)
		{
			if (old_counts == null)
				throw new ArgumentNullException("old_counts");
			if (counts == null)
				throw new ArgumentNullException("counts");

			int max_prime = 0;

			foreach (DistinctPrimeComposite factor in old_counts.Keys)
			{
				if (factor.max_prime_factor <= max_prime_to_keep)
				{
					counts.Add(factor, old_counts[factor]);
					if (factor.max_prime_factor > max_prime)
						max_prime = factor.max_prime_factor;
				}
			}

			return max_prime;
		}

		protected override string CalculateSolution()
		{
			long result_count = 1;
			List<Queue<DistinctPrimeComposite>> max_prime_factors = new List<Queue<DistinctPrimeComposite>>(Enumerable.Repeat((Queue<DistinctPrimeComposite>)null, c_max + 1));

			for (int c = c_min; c <= c_max; ++c)
			{
				DistinctPrimeComposite new_dpc = DistinctPrimeComposite.GetDistinctPrimeComposite(c);
				if (new_dpc.max_prime_factor == 1)
				{
					result_count *= 2;
					if (result_count >= modulo_base)
						result_count -= modulo_base;
				}
				else if (c + new_dpc.max_prime_factor <= c_max || c - new_dpc.max_prime_factor >= c_min)
				{
					if (max_prime_factors[new_dpc.max_prime_factor] == null)
					{
						max_prime_factors[new_dpc.max_prime_factor] = new Queue<DistinctPrimeComposite>();
					}
					max_prime_factors[new_dpc.max_prime_factor].Enqueue(new_dpc);
				}
			}

			Dictionary<DistinctPrimeComposite, long> empty_counts = null;
			Queue<KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>> composite_counts = new Queue<KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>>();
			List<int> max_prime_count = new List<int>();
			int max_prime_to_keep = 1;

			{
				DistinctPrimeComposite dpc1 = null, dpc2 = null;
				int max_prime = max_prime_factors.Count - 1;
				while (max_prime > 0)
				{
					if (max_prime_factors[max_prime] == null)
					{
						--max_prime;
					}
					else if (max_prime_factors[max_prime].Count == 0)
					{
						max_prime_factors[max_prime] = null;
						--max_prime;
					}
					else if (dpc1 == null)
					{
						dpc1 = max_prime_factors[max_prime].Dequeue();
					}
					else
					{
						dpc2 = max_prime_factors[max_prime].Dequeue();

						Dictionary<DistinctPrimeComposite, long> counts = new Dictionary<DistinctPrimeComposite, long>();
						int real_max_prime = CountCompositeFactors(dpc1, dpc2, counts);

						composite_counts.Enqueue(new KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>(real_max_prime, counts));

						while (max_prime_count.Count <= real_max_prime)
							max_prime_count.Add(0);
						++max_prime_count[real_max_prime];
						if (max_prime > max_prime_to_keep)
							max_prime_to_keep = max_prime;

						dpc1 = dpc2 = null;
					}
				}

				max_prime_factors = null;

				if (dpc1 != null)
				{
					KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>> old_counts = composite_counts.Dequeue();
					--max_prime_count[old_counts.Key];

					while (max_prime_count[max_prime_to_keep] <= 0)
						--max_prime_to_keep;


					Dictionary<DistinctPrimeComposite, long> counts = new Dictionary<DistinctPrimeComposite, long>();
					int real_max_prime = CountCompositeFactorAndCount(dpc1, old_counts.Value, counts, max_prime_to_keep);

					composite_counts.Enqueue(new KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>(real_max_prime, counts));

					++max_prime_count[real_max_prime];

					empty_counts = old_counts.Value;
					empty_counts.Clear();
				}
				else
				{
					empty_counts = new Dictionary<DistinctPrimeComposite, long>();
				}
			}

			while (composite_counts.Count > 1)
			{
				KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>> old_counts1 = composite_counts.Dequeue();
				--max_prime_count[old_counts1.Key];
				KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>> old_counts2 = composite_counts.Dequeue();
				--max_prime_count[old_counts2.Key];

				while (max_prime_count[max_prime_to_keep] <= 0)
					--max_prime_to_keep;


				Dictionary<DistinctPrimeComposite, long> counts = empty_counts;

				int real_max_prime = CountCompositeCounts(old_counts1.Value, old_counts2.Value, counts, max_prime_to_keep);

				while (real_max_prime > 1 && composite_counts.Count > 0 && (counts.Count <= old_counts1.Value.Count || counts.Count <= old_counts2.Value.Count))
				{
					if (old_counts1.Value.Count > old_counts2.Value.Count)
						empty_counts = old_counts1.Value;
					else
						empty_counts = old_counts2.Value;
					empty_counts.Clear();

					old_counts1 = new KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>(real_max_prime, counts);
					old_counts2 = composite_counts.Dequeue();
					--max_prime_count[old_counts2.Key];

					while (max_prime_count[max_prime_to_keep] <= 0)
						--max_prime_to_keep;

					counts = empty_counts;

					real_max_prime = CountCompositeCounts(old_counts1.Value, old_counts2.Value, counts, max_prime_to_keep);
				}

				if (real_max_prime > 1)
				{
					composite_counts.Enqueue(new KeyValuePair<int, Dictionary<DistinctPrimeComposite, long>>(real_max_prime, counts));
					++max_prime_count[real_max_prime];
				}
				else
				{
					result_count *= counts[DistinctPrimeComposite.RootFactor];
					result_count %= modulo_base;
				}

				if (old_counts1.Value.Count > old_counts2.Value.Count)
					empty_counts = old_counts1.Value;
				else
					empty_counts = old_counts2.Value;
				empty_counts.Clear();
			}

			return (result_count - 1).ToString();
		}
	}
}
