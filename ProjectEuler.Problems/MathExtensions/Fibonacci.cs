using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace ProjectEuler.MathExtensions
{
	internal static class Fibonacci
	{
		static private ReaderWriterLockSlim _accessFibonacci = new ReaderWriterLockSlim();
		static private List<ulong> _fibonaccis = InitFibonacci();

		static private List<ulong> InitFibonacci()
		{
			List<ulong> f = new List<ulong>();
			f.Add(1);
			f.Add(1);
			return f;
		}

		static public ulong GetFibonacci(int i)
		{
			if (i >= _fibonaccis.Count)
			{
				_accessFibonacci.EnterWriteLock();
				while (i >= _fibonaccis.Count)
					_fibonaccis.Add(_fibonaccis[_fibonaccis.Count - 2] + _fibonaccis[_fibonaccis.Count - 1]);
				_accessFibonacci.ExitWriteLock();
			}

			_accessFibonacci.EnterReadLock();
			ulong fib = _fibonaccis[i];
			_accessFibonacci.ExitReadLock();

			return fib;
		}
	}
}
