using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ProjectEuler.MathExtensions
{
	internal static class Primes
	{
		#region Global properties for managing the size of the _primes cache.
		static private ReaderWriterLockSlim _accessPrimes = new ReaderWriterLockSlim();
		static private List<int> _primes = CreateIntListWithOneValue(2);
		static private List<int> _primeTesters = CreateIntListWithOneValue(1);
		static private int _interval = 2;
		static private int _nextInterval = 6;
		static private int _maxTest = 2;
		static private int _nextPrime = 0;
		static private int _max_cached_prime = 0;
		#endregion

		#region Special "pump the primes" thread to expand the size of the prime cache...
		static private SemaphoreSlim _managePump = new SemaphoreSlim(1);
		static private int __maxIndex = -1;
		static private int _maxIndex
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return __maxIndex; }
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				SpinWait spinWait = new SpinWait();

				while (value > __maxIndex)
				{
					int tmp1 = __maxIndex;
					int newMaxIndex;
					if (value > tmp1)
						newMaxIndex = value;
					else
						newMaxIndex = tmp1;
					int tmp2 = Interlocked.CompareExchange(ref __maxIndex, newMaxIndex, tmp1);
					if (tmp1 == tmp2)
					{
						if (newMaxIndex > tmp1)
							StartPrimeThread();
						break;
					}
					spinWait.SpinOnce();
				}
			}
		}
		public static int CacheSize
		{ [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return _primes.Count; } }
		public static int MaxCachedPrime
		{ [MethodImpl(MethodImplOptions.AggressiveInlining)] get { return _max_cached_prime; } }
		static private Thread _primePumpThread = null;
		#endregion

		#region Thread caching variables
		[ThreadStatic]
		static private List<int> _threadPrimes;
		[ThreadStatic]
		static private int _cachedMin, _cachedMax;
		#endregion

		private static List<int> CreateIntListWithOneValue(int n)
		{
			List<int> list = new List<int>();
			list.Add(n);
			return list;
		}

		static private void StartPrimeThread()
		{
			if (_primePumpThread == null && _maxIndex + 8 * (2 * 3 * 5 * 7 * 11) > _primes.Count)
			{
				_managePump.Wait();
				if (_primePumpThread == null)
				{
					_primePumpThread = new Thread(PumpThePrimes);
					_primePumpThread.Name = "Prime Pumper Thread";
					_primePumpThread.Priority = ThreadPriority.AboveNormal;
					_primePumpThread.Start();
				}
				_managePump.Release();
			}
		}

		static public bool Is_SPRP_ProbablePrime(long a, long n)
		{
			if (n <= 2)
			{
				if (n == 2)
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			if ((n & 0x1) == 0)
			{
				return false;
			}

			long d = n - 1;
			long s = 0;
			while ((d & 0x1) == 0)
			{
				++s;
				d = d >> 1;
			}

			long a_tothe_dth_power = a_ToThe_d_Mod_n(a, d, n);

			if (a_tothe_dth_power == 1 || a_tothe_dth_power == n - 1)
			{
				return true;
			}

			for (int r = 1; r < s; ++r)
			{
				a_tothe_dth_power = Square_Mod_n(a_tothe_dth_power, n);
				if (a_tothe_dth_power == n - 1)
				{
					return true;
				}
			}

			return false;
		}

		static public bool Is_Miller_Rabin_ProbablePrime(long a, long d, long r, long n)
		{
			long x = a_ToThe_d_Mod_n(a, d, n);
			if (x == 1 || x == n - 1)
				return true;
			while (--r > 0)
			{
				x = Square_Mod_n(x, n);
				if (x == n - 1)
					return true;
				if (x == 1)
					return false;
			}
			return false;
		}

		static private bool Is_Miller_Rabin_Prime_Greater_Than_1373653(long n, long d, long r)
		{
			if (n < 9080191)
			{
				return Is_Miller_Rabin_ProbablePrime(31, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(73, d, r, n);
			}
			else if (n < 25326001)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(3, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(5, d, r, n);
			}
			else
				return Is_Miller_Rabin_Prime_Greater_Than_25326001(n, d, r);
		}

		static private bool Is_Miller_Rabin_Prime_Greater_Than_25326001(long n, long d, long r)
		{
			if (n < 3215031751)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(3, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(5, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(7, d, r, n);
			}
			else if(n < 4759123141)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(7, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(61, d, r, n);
			}
			else
				return Is_Miller_Rabin_Prime_Greater_Than_4759123141(n, d, r);
		}

		static private bool Is_Miller_Rabin_Prime_Greater_Than_4759123141(long n, long d, long r)
		{
			if (n < 1122004669633)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(13, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(23, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(1662803, d, r, n);
			}
			else if (n < 2152302898747)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(3, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(5, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(7, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(11, d, r, n);
			}
			else
				return Is_Miller_Rabin_Prime_Greater_Than_2152302898747(n, d, r);
		}

		static private bool Is_Miller_Rabin_Prime_Greater_Than_2152302898747(long n, long d, long r)
		{
			if (n < 3474749660383)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(3, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(5, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(7, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(11, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(13, d, r, n);
			}
			else if (n < 341550071728321)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(3, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(5, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(7, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(11, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(13, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(17, d, r, n);
			}
			else
				return Is_Miller_Rabin_Prime_Greater_Than_341550071728321(n, d, r);
		}

		static private bool Is_Miller_Rabin_Prime_Greater_Than_341550071728321(long n, long d, long r)
		{
			if (n < 3825123056546413051)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(3, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(5, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(7, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(11, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(13, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(17, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(19, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(23, d, r, n);
			}
			else
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(3, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(5, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(7, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(11, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(13, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(17, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(19, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(23, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(29, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(31, d, r, n) &&
					Is_Miller_Rabin_ProbablePrime(37, d, r, n);
		}

		static public bool Is_Miller_Rabin_Prime(long n)
		{
			long d = n - 1, r = 0;
			while ((d & 0x1) == 0)
			{
				d = d >> 1;
				++r;
			}

			if (n < 2047)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n);
			}
			else if (n < 1373653)
			{
				return Is_Miller_Rabin_ProbablePrime(2, d, r, n)
					&& Is_Miller_Rabin_ProbablePrime(3, d, r, n);
			}
			else
				return Is_Miller_Rabin_Prime_Greater_Than_1373653(n, d, r);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private int Find_Exponential_Shift(long n)
		{
			long bit_mask = 0x100000000;
			int shift = 31;

			for (int next_shift = 16; next_shift > 0;)
			{
				if (n < bit_mask)
				{
					bit_mask = bit_mask >> next_shift;
					shift += next_shift;
					next_shift /= 2;
				}
				else if (n >= 2 * bit_mask)
				{
					bit_mask = bit_mask << next_shift;
					shift -= next_shift;
					next_shift /= 2;
				}
				else
				{
					break;
				}
			}

			return shift;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private int Jacobi_Symbol(long k, long n)
		{
			int sym = 1;

			while (true)
			{
				k = k % n;
				if (k < 0)
				{
					k = k + n;
				}

				if (k > 0)
				{
					while ((k & 0x3) == 0)
					{
						k = k >> 2;
					}

					if ((k & 0x1) == 0)
					{
						k = k >> 1;
						if ((n & 0x7) == 3 || (n & 0x7) == 5)
						{
							sym = -sym;
						}
					}
				}

				if (k == 1)
				{
					return sym;
				}

				if (Ratio.GreatestCommonDivisor(k, n) != 1)
				{
					return 0;
				}

				if ((k & 0x3) == 3 && (n & 0x3) == 3)
					sym = -sym;

				long temp = k;
				k = n;
				n = temp;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private long Multiply_Mod_n(long a, long b, long n)
		{
			a = a % n;
			b = b % n;
			if (a == 0 || b == 0)
				return 0;
			if (a < 0)
				a += n;
			if (b < 0)
				b += n;

			if (long.MaxValue / a > b)
			{
				return (a * b) % n;
			}

			long a_mult_big = a, a_mult_little = a, b_mult_big = b, b_mult_little = b;
			long a_count_mult_big = 1, a_count_mult_little = 0, b_count_mult_big = 1, b_count_mult_little = 0;

			while (long.MaxValue / a_mult_big <= b_mult_big)
			{
				if (a_mult_big > b_mult_big)
				{
					if (a_mult_big % 2 == 0)
					{
						a_count_mult_big = 2 * a_count_mult_big + a_count_mult_little;
						a_mult_big /= 2;
						a_mult_little = a_mult_big - 1;
					}
					else
					{
						a_count_mult_little = 2 * a_count_mult_little + a_count_mult_big;
						a_mult_little /= 2;
						a_mult_big = a_mult_little + 1;
					}
				}
				else
				{
					if (b_mult_big % 2 == 0)
					{
						b_count_mult_big = 2 * b_count_mult_big + b_count_mult_little;
						b_mult_big /= 2;
						b_mult_little = b_mult_big - 1;
					}
					else
					{
						b_count_mult_little = 2 * b_count_mult_little + b_count_mult_big;
						b_mult_little /= 2;
						b_mult_big = b_mult_little + 1;
					}
				}
			}

			long accumulator = (a_count_mult_big * ((b_count_mult_big * ((a_mult_big * b_mult_big) % n)) % n)) % n;
			accumulator += (a_count_mult_big * ((b_count_mult_little * ((a_mult_big * b_mult_little) % n)) % n)) % n;
			accumulator %= n;
			accumulator += (a_count_mult_little * ((b_count_mult_big * ((a_mult_little * b_mult_big) % n)) % n)) % n;
			accumulator %= n;
			accumulator += (a_count_mult_little * ((b_count_mult_little * ((a_mult_little * b_mult_little) % n)) % n)) % n;
			return accumulator % n;
		}

		static private long Square_Mod_n(long a, long n)
		{
			if (a <= 0x7FFFFFFF)
			{
				return (a * a) % n;
			}

			long mult_big = a, mult_little = a;
			long count_mult_big = 1, count_mult_little = 0;

			while (mult_big > 0x7FFFFFFF)
			{
				if (mult_big % 2 == 0)
				{
					count_mult_big = 2 * count_mult_big + count_mult_little;
					mult_big /= 2;
					mult_little = mult_big - 1;
				}
				else
				{
					count_mult_little = 2 * count_mult_little + count_mult_big;
					mult_little /= 2;
					mult_big = mult_little + 1;
				}
			}

			long accumulator = (count_mult_big * ((count_mult_big * ((mult_big * mult_big) % n)) % n)) % n;
			accumulator += (2 * ((count_mult_big * ((count_mult_little * ((mult_big * mult_little) % n)) % n)) % n)) % n;
			accumulator %= n;
			accumulator += (count_mult_little * ((count_mult_little * ((mult_little * mult_little) % n)) % n)) % n;
			accumulator %= n;
			return accumulator % n;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private long a_ToThe_d_Mod_n(long a, long d, long n)
		{
			int shift = Find_Exponential_Shift(d);
			long a_tothe_dth_power = 1;
			d = d << shift;
			while (d != 0)
			{
				if (((ulong)d & 0x8000000000000000) != 0)
				{
					a_tothe_dth_power = Multiply_Mod_n(a_tothe_dth_power, a, n);
					d ^= unchecked((long)0x8000000000000000);
				}
				else
				{
					a_tothe_dth_power = Square_Mod_n(a_tothe_dth_power, n);
					d = d << 1;
				}
			}

			return a_tothe_dth_power;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private bool Is_Perfect_Square(long n)
		{
			if (n >= 9223372030926249001 || n <= 0)
			{
				if (n > 9223372030926249001 || n < 0)
				{
					// Outside the range of possible long perfect squares...
					return false;
				}
				return true;
			}

			long _sq_test = n >> 4;
			// 3037000499 is the  sqrt of 9223372030926249001.
			for (long min_test = 0, max_test = (n < 3037000499 ? n : 3037000499), test_square;
						min_test < max_test - 1;
						_sq_test = (n - test_square) / (2 * _sq_test) + _sq_test)
			{
				if (_sq_test >= max_test)
					_sq_test = max_test - 1;
				else if (_sq_test <= min_test)
					_sq_test = min_test + 1;

				test_square = _sq_test * _sq_test;

				if (test_square < n)
					min_test = _sq_test;
				else if (test_square > n)
					max_test = _sq_test;
				else
					return true;
			}

			return false;
		}

		static private bool Is_Baillie_PSW_Prime(long n)
		{
			if (!Is_SPRP_ProbablePrime(2, n))
			{
				return false;
			}

			if (Is_Perfect_Square(n))
			{
				return false;
			}

			int sign = 1;
			long D = 5;
			while (Jacobi_Symbol(sign * D, n) != -1)
			{
				D += 2;
				sign = -sign;
			}
			D = sign * D;
			long Q = ((1 - D) / 4 + n) % n;
			if (Q < 0)
			{
				Q += n;
			}

			Stack<long> k_stack = new Stack<long>();
			for (long k = n + 1; k > 1; k = k >> 1)
			{
				k_stack.Push(k);
			}

			long n_mod = n << 1;
			long Uk = 1, Vk = 1, Qk = Q;
			while (k_stack.Count > 0)
			{
				long k = k_stack.Pop();

				long U2k = Multiply_Mod_n(Uk, Vk, n_mod),
					V2k = (Square_Mod_n(Vk, n_mod) + 2 * n_mod - 2 * Qk) % n_mod,
					Q2k = Square_Mod_n(Qk, n_mod);

				if ((k & 0x1) == 0)
				{
					Uk = U2k;
					Vk = V2k;
					Qk = Q2k;
				}
				else
				{
					Uk = ((U2k + V2k) >> 1) % n_mod;
					Vk = ((D * U2k + V2k) / 2) % n_mod;
					if (Vk < 0)
						Vk += n_mod;
					Qk = Multiply_Mod_n(Q2k, Q, n_mod);

					if (k_stack.Count == 1 && (Qk % n) != ((Q * Jacobi_Symbol(Q, n) + n) % n))
					{
						return false;
					}

					if (Vk < 0)
					{
						Vk += n_mod;
					}
				}
			}

			return (Uk % n) == 0 && (Vk % n) == ((Q << 1) % n);
		}

		private const int MAX_ADD = int.MaxValue - 4;
		private const int MAX_SHIFT = int.MaxValue / 2;
		static public bool IsPrime(long primetest, bool requireExpansion = true)
		{
			if (primetest < 2)
			{
				return false;
			}

			if (_cachedMin < 0)
				_cachedMin = 0;
			if (_cachedMax <= 0)
				_cachedMax = 1;

			if (GetPrime(_cachedMin, requireExpansion) != primetest && GetPrime(_cachedMax, requireExpansion) != primetest)
			{
				#region Don't wait for filling primes if the primetest is possibly VERY large.
				if (!requireExpansion && primetest > _threadPrimes[_threadPrimes.Count - 1])
				{
					return Is_Miller_Rabin_Prime(primetest);
				}
				#endregion

				while (GetPrime(_cachedMin, requireExpansion) > primetest)
				{
					bool decrementTypeSubtract = (_cachedMax <= _cachedMin + 3) && (_cachedMin >= 4);
					_cachedMax = _cachedMin;
					if (decrementTypeSubtract)
						_cachedMin -= 4;
					else
						_cachedMin >>= 1;
				}
				while (GetPrime(_cachedMax, requireExpansion) < primetest)
				{
					bool incrementTypeAdd = (_cachedMax <= _cachedMin + 3) && (_cachedMax <= MAX_ADD);
					_cachedMin = _cachedMax;
					if (incrementTypeAdd)
						_cachedMax += 4;
					else if (_cachedMax <= MAX_SHIFT)
					{
						if (_cachedMax > 8192 && (2 * _cachedMax > _primes.Count))
						{
							if (_cachedMax + 8192 < _threadPrimes.Count)
								_cachedMax = _threadPrimes.Count - 1;
							else
								_cachedMax = _cachedMax + 8192;
						}
						else
						{
							_cachedMax *= 2;
						}
					}
					else
					{
						_cachedMax = int.MaxValue;
						break;
					}
					if (!requireExpansion && _cachedMax >= _threadPrimes.Count)
						_cachedMax = _threadPrimes.Count - 1;
				}
				while (_cachedMax > _cachedMin + 1)
				{
					int test = (_cachedMin + _cachedMax) / 2;
					if (GetPrime(test, requireExpansion) <= primetest)
					{
						_cachedMin = test;
					}
					if (GetPrime(test, requireExpansion) >= primetest)
					{
						_cachedMax = test;
					}
				}
			}

			return GetPrime(_cachedMin, requireExpansion) == primetest || GetPrime(_cachedMax, requireExpansion) == primetest;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static public int IndexOfPrimeAtLeast(int primetest)
		{
			if (IsPrime(primetest, true) && primetest == GetPrime(_cachedMin))
				return _cachedMin;
			else
				return _cachedMax;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static public int IndexOfPrimeAtMost(int primetest)
		{
			if (IsPrime(primetest, true) && primetest == GetPrime(_cachedMax))
				return _cachedMax;
			else
				return _cachedMin;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private void WaitForPrimedPump(int n)
		{
			SpinWait.SpinUntil(() =>
			{
				Thread.MemoryBarrier();
				return n < _primes.Count;
			});
		}

		static public int GetPrime(int n, bool requireExpansion = true)
		{
			if (n < 0)
				n = 0;

			if (n > _maxIndex && (requireExpansion || n > _primes.Count))
				_maxIndex = n;

			if (_threadPrimes != null && n < _threadPrimes.Count)
				return _threadPrimes[n];

			if (n >= _primes.Count)
			{
				WaitForPrimedPump(n);
			}

			if (_threadPrimes == null)
				_threadPrimes = new List<int>(50000);

			if (_threadPrimes.Capacity <= _primes.Count)
				_threadPrimes.Capacity = _primes.Count * 5 / 4;

			int copy_count = n + 128;
			if (copy_count > _primes.Count)
				copy_count = _primes.Count;
			copy_count -= _threadPrimes.Count;

			_accessPrimes.EnterReadLock();
			_threadPrimes.AddRange(_primes.GetRange(_threadPrimes.Count, copy_count));
			_accessPrimes.ExitReadLock();

			return _threadPrimes[n];
		}

		static private void PumpThePrimes()
		{
			Queue<int> resultPrimes = new Queue<int>();
			int threshold = 0;

			for (bool proceed = true; proceed;)
			{
				#region Create results for the next interval assigned to this process.
				for (int i = 0; i < _primeTesters.Count; i++)
				{
					int test = _maxTest + _primeTesters[i];

					for (int p = _nextPrime; test % _primes[p] != 0; ++p)
					{
						if (_primes[p] * _primes[p] > test)
						{
							resultPrimes.Enqueue(test);
							break;
						}
					}
				}
				_maxTest += _interval;
				#endregion

				#region If this thread has the next set of results to be entered, it should enter them...
				if (resultPrimes.Count > 0)
				{
					_accessPrimes.EnterWriteLock();
					while (resultPrimes.Count > 0)
						_primes.Add(resultPrimes.Dequeue());
					_accessPrimes.ExitWriteLock();
					_max_cached_prime = _primes[_primes.Count - 1];
				}
				#endregion

				#region Assign new interval(s) for this thread, if required
				ExpandTesterSize();
				#endregion

				Thread.MemoryBarrier();
				if (threshold <= _primes.Count && (threshold = _maxIndex + 8 * (2 * 3 * 5 * 7 * 11)) <= _primes.Count)
				{
					_managePump.Wait();
					if ((threshold = _maxIndex + 8 * (2 * 3 * 5 * 7 * 11)) <= _primes.Count)
					{
						_primePumpThread = null;
						proceed = false;
					}
					_managePump.Release();
				}
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static private void ExpandTesterSize()
		{
			if (_nextPrime <= 4 && _maxTest % _nextInterval == 0 && _primes.Count > _nextPrime + 1)
			{
				_accessPrimes.EnterWriteLock();

				List<int> new_sieve = new List<int>();

				int p_ignore = 0, prime_ignore = _primeTesters[0] * _primes[_nextPrime];
				for (int maxTesters = 0; maxTesters < _nextInterval; maxTesters += _interval)
					for (int i = 0; i < _primeTesters.Count; i++)
					{
						int test = maxTesters + _primeTesters[i];
						if (test != prime_ignore)
							new_sieve.Add(test);
						else if (p_ignore + 1 < _primeTesters.Count)
							prime_ignore = _primeTesters[++p_ignore] * _primes[_nextPrime];
					}


				_primeTesters = new_sieve;
				_interval = _nextInterval;
				++_nextPrime;
				_nextInterval *= _primes[_nextPrime];

				_accessPrimes.ExitWriteLock();
			}
		}

		static public List<int> BuildSieve(int maxInterval)
		{
			_accessPrimes.EnterWriteLock();
			List<int> sieve = new List<int>(_primeTesters);
			int next_prime = _nextPrime;
			int interval = _interval,
				next_interval = _nextInterval;
			_accessPrimes.ExitWriteLock();

			if (next_interval > maxInterval) return sieve;
			List<int> new_sieve = new List<int>();
			while (next_interval <= maxInterval)
			{
				int p_ignore = 0;
				int prime_ignore = sieve[0] * _primes[next_prime];
				for (int max_test = 0; max_test < next_interval; max_test += interval)
					for (int i = 0; i < sieve.Count; i++)
					{
						int test = max_test + sieve[i];
						if (test != prime_ignore)
							new_sieve.Add(test);
						else if (p_ignore + 1 < sieve.Count)
							prime_ignore = sieve[++p_ignore] * _primes[next_prime];
					}

				List<int> old_sieve = sieve;
				sieve = new_sieve;
				new_sieve = old_sieve;
				old_sieve.Clear();

				interval = next_interval;
				++next_prime;
				next_interval *= _primes[next_prime];
			}

			return sieve;
		}

		static public List<long> BuildSieve(long maxInterval)
		{
			_accessPrimes.EnterWriteLock();
			List<long> sieve = new List<long>();
			for (int i = 0; i < _primeTesters.Count; ++i)
				sieve.Add(_primeTesters[i]);
			int next_prime = _nextPrime;
			long interval = _interval,
				next_interval = _nextInterval;
			_accessPrimes.ExitWriteLock();

			if (next_interval > maxInterval) return sieve;
			List<long> new_sieve = new List<long>();
			while (next_interval <= maxInterval)
			{
				int p_ignore = 0;
				long prime_ignore = sieve[0] * GetPrime(next_prime);
				for (long max_test = 0; max_test < next_interval; max_test += interval)
					for (int i = 0; i < sieve.Count; i++)
					{
						long test = max_test + sieve[i];
						if (test != prime_ignore)
							new_sieve.Add(test);
						else if (p_ignore + 1 < sieve.Count)
							prime_ignore = sieve[++p_ignore] * GetPrime(next_prime);
					}

				List<long> old_sieve = sieve;
				sieve = new_sieve;
				new_sieve = old_sieve;
				old_sieve.Clear();

				interval = next_interval;
				++next_prime;
				next_interval *= GetPrime(next_prime);
			}

			return sieve;
		}

		static public bool HaltPump()
		{
			if (_primePumpThread != null && _primePumpThread.IsAlive)
			{
				_primePumpThread.Abort();
			}
			return _primePumpThread == null || !_primePumpThread.IsAlive;
		}
	}
}
