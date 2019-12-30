using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler.MathExtensions
{
	internal struct Ratio
	{
		private BigInt _numerator, _denominator;

		public Ratio(BigInt number)
		{
			_numerator = number;
			_denominator = 1;
		}

		public static implicit operator Ratio(BigInt number)
		{
			return new Ratio(number);
		}

		public static implicit operator Ratio(int number)
		{
			return new Ratio(number);
		}

		public Ratio(BigInt numerator, BigInt denominator)
		{
			if (denominator == 0)
				throw new ArgumentOutOfRangeException("denominator", "denominator equals 0.");

			_numerator = numerator;

			if (denominator > 0)
			{
				_numerator = numerator;
				_denominator = denominator;
			}
			else
			{
				_numerator = -numerator;
				_denominator = -denominator;
			}

			BigInt gcd = GreatestCommonDivisor(_denominator, _numerator);
			if (gcd > 1)
			{
				_numerator /= gcd;
				_denominator /= gcd;
			}
		}

		static public long GreatestCommonDivisor(int bigger, int smaller)
		{
			if (bigger < 0)
				bigger = -bigger;

			if (smaller <= 0)
			{
				if (smaller == 0)
					return bigger;
				else
					smaller = -smaller;
			}

			for (long remainder = bigger % smaller; remainder > 0; remainder = bigger % smaller)
			{
				bigger = smaller;
				smaller = remainder;
			}

			return smaller;
		}

		static public long GreatestCommonDivisor(long bigger, long smaller)
		{
			if (bigger < 0)
				bigger = -bigger;

			if (smaller <= 0)
			{
				if (smaller == 0)
					return bigger;
				else
					smaller = -smaller;
			}

			for (long remainder = bigger % smaller; remainder > 0; remainder = bigger % smaller)
			{
				bigger = smaller;
				smaller = remainder;
			}

			return smaller;
		}

		static public BigInt GreatestCommonDivisor(BigInt bigger, BigInt smaller)
		{
			if (bigger < 0)
				bigger = -bigger;

			if (smaller <= 0)
			{
				if (smaller == 0)
					return bigger;
				else
					smaller = -smaller;
			}

			for (BigInt remainder = bigger % smaller; remainder > 0; remainder = bigger % smaller)
			{
				bigger = smaller;
				smaller = remainder;
			}

			return smaller;
		}

		public override bool Equals(object obj)
		{
			if (obj is Ratio)
				return this == (Ratio)obj;

			return base.Equals(obj);
		}

		public static bool operator ==(Ratio a, Ratio b)
		{
			return (a._numerator == b._numerator && a._denominator == b._denominator);
		}

		public static bool operator !=(Ratio a, Ratio b)
		{
			return (a._numerator != b._numerator || a._denominator != b._denominator);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static Ratio operator +(Ratio a, Ratio b)
		{
			BigInt gcd = GreatestCommonDivisor(a._denominator, b._denominator);
			BigInt a_d = a._denominator, b_d = b._denominator;

			if (gcd > 1)
			{
				a_d = a._denominator / gcd;
				b_d = b._denominator / gcd;
			}
			else
			{
				a_d = a._denominator;
				b_d = b._denominator;
			}

			return new Ratio(a._numerator * b_d + b._numerator * a_d, a._denominator * b_d);
		}

		public static Ratio operator -(Ratio a, Ratio b)
		{
			BigInt gcd = GreatestCommonDivisor(a._denominator, b._denominator);
			BigInt a_d = a._denominator, b_d = b._denominator;

			if (gcd > 1)
			{
				a_d = a._denominator / gcd;
				b_d = b._denominator / gcd;
			}
			else
			{
				a_d = a._denominator;
				b_d = b._denominator;
			}

			return new Ratio(a._numerator * b_d - b._numerator * a_d, a._denominator * b_d);
		}

		public static Ratio operator *(Ratio a, Ratio b)
		{
			BigInt numerator, denominator;

			BigInt gcd = GreatestCommonDivisor(b._denominator, a._numerator);
			if (gcd > 1)
			{
				numerator = a._numerator / gcd;
				denominator = b._denominator / gcd;
			}
			else
			{
				numerator = a._numerator;
				denominator = b._denominator;
			}

			gcd = GreatestCommonDivisor(a._denominator, b._numerator);
			if (gcd > 1)
			{
				numerator *= b._numerator / gcd;
				denominator *= a._denominator / gcd;
			}
			else
			{
				numerator *= b._numerator;
				denominator *= a._denominator;
			}

			return new Ratio(numerator, denominator);
		}

		public static Ratio operator /(Ratio a, Ratio b)
		{
			BigInt numerator, denominator;

			BigInt gcd = GreatestCommonDivisor(b._numerator, a._numerator);
			if (gcd > 1)
			{
				numerator = a._numerator / gcd;
				denominator = b._numerator / gcd;
			}
			else
			{
				numerator = a._numerator;
				denominator = b._numerator;
			}

			gcd = GreatestCommonDivisor(a._denominator, b._denominator);
			if (gcd > 1)
			{
				numerator *= b._denominator / gcd;
				denominator *= a._denominator / gcd;
			}
			else
			{
				numerator *= b._denominator;
				denominator *= a._denominator;
			}

			return new Ratio(numerator, denominator);
		}
	}
}