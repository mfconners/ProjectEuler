using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEuler.MathExtensions
{
	internal struct BigInt
	{
		private bool _isNegative;
		private UInt32 _number;
		private List<UInt32> _32bitdigits;

		public BigInt(UInt32 num = 0)
		{
			_isNegative = false;
			_number = num;
			_32bitdigits = null;
		}

		public static implicit operator BigInt(UInt32 num)
		{
			return new BigInt(num);
		}

		public BigInt(Int32 num = 0)
		{
			if (num < 0)
			{
				_isNegative = true;
				num = -num;
			}
			else
			{
				_isNegative = false;
			}
			_number = (UInt32) (num);
			_32bitdigits = null;
		}

		public static implicit operator BigInt(Int32 num)
		{
			return new BigInt(num);
		}

		public BigInt(BigInt num)
		{
			_isNegative = num._isNegative;
			_number = num._number;
			_32bitdigits = num._32bitdigits;
		}

		private int Num32BitDigits
		{
			get
			{
				if (_32bitdigits == null)
					return 0;
				else
					return _32bitdigits.Count;
			}
		}

		private static BigInt DoDivision(BigInt Dividend, BigInt Divisor, bool modulo = false)
		{
			#region Identities & Init
			if (Divisor == 0)
				throw new DivideByZeroException();
			if (Dividend == 0)
				return Dividend;

			bool isPositive = Dividend._isNegative == Divisor._isNegative;

			BigInt posDividend = Dividend;
			if (Dividend._isNegative)
				posDividend = -Dividend;
			BigInt posDivisor = Divisor;
			if (Divisor._isNegative)
				posDivisor = -Divisor;

			if (posDivisor == 1)
			{
				if (modulo)
					return 0;
				else if (isPositive)
					return posDividend;
				else
					return -posDividend;
			}

			if (posDividend == posDivisor)
			{
				if (modulo)
					return 0;
				else if (isPositive)
					return 1;
				else
					return -1;
			}

			if (posDividend < posDivisor)
			{
				if (!modulo)
					return 0;
				else if (isPositive)
					return Dividend;
				else
					return (Dividend + Divisor);
			}
			#endregion

			BigInt bigOne = 1;

			int shift = (posDividend.Num32BitDigits - posDivisor.Num32BitDigits) * 32;
			while (posDividend > (posDivisor << (shift + 1)))
				++shift;

			BigInt result = 0;
			while (shift >= 0)
			{
				BigInt shiftedDivisor = (posDivisor << shift);
				if (posDividend > shiftedDivisor)
				{
					if (!modulo)
						result = result + (bigOne << shift);
					posDividend = posDividend - shiftedDivisor;
				}
				--shift;
			}

			if (modulo)
				return posDividend;
			else
				return result;
		}

		public override int GetHashCode()
		{
			int result = (int) _number;
			for (int i = 0; i < Num32BitDigits; ++i)
			{
				result = (result << 3) | (result >> 29);
				result = result ^ (int) _32bitdigits[i];
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			if (obj is BigInt)
				return this == (BigInt) obj;

			return base.Equals(obj);
		}

		public static bool operator ==(BigInt a, BigInt b)
		{
			if (a.Num32BitDigits != b.Num32BitDigits)
				return false;
			if (a._number != b._number)
				return false;
			if (a.Num32BitDigits == 0)
				return (a._number == 0) || (a._isNegative == b._isNegative);
			if (a._isNegative ^ b._isNegative)
				return false;

			for (int i = 0; i < a.Num32BitDigits; ++i)
				if (a._32bitdigits[i] != b._32bitdigits[i])
					return false;

			return true;
		}

		public static bool operator !=(BigInt a, BigInt b)
		{
			return !(a == b);
		}

		public static bool operator <(BigInt a, BigInt b)
		{
			if (a.Num32BitDigits == 0 && b.Num32BitDigits == 0 && a._number == 0 && b._number == 0)
				return false;

			if (a._isNegative ^ b._isNegative)
				return a._isNegative;
			if (a.Num32BitDigits > b.Num32BitDigits)
				return a._isNegative;
			if (a.Num32BitDigits < b.Num32BitDigits)
				return !a._isNegative;

			for (int i = a.Num32BitDigits - 1; i >= 0; --i)
			{
				if (a._32bitdigits[i] > b._32bitdigits[i])
					return a._isNegative;
				if (a._32bitdigits[i] < b._32bitdigits[i])
					return !a._isNegative;
			}

			if (a._number > b._number)
				return a._isNegative;
			if (a._number < b._number)
				return !a._isNegative;
			return false;
		}

		public static bool operator >(BigInt a, BigInt b)
		{
			return b < a;
		}

		public static bool operator <=(BigInt a, BigInt b)
		{
			return !(b < a);
		}

		public static bool operator >=(BigInt a, BigInt b)
		{
			return !(a < b);
		}

		public static BigInt operator -(BigInt a)
		{
			if (a == 0)
				return a;

			BigInt result = new BigInt(a._number);
			result._isNegative = !a._isNegative;
			result._32bitdigits = a._32bitdigits;

			return result;
		}

		public static BigInt operator *(BigInt a, BigInt b)
		{
			#region Check the identities
			if (a == 0)
				return a;
			if (b == 0)
				return b;
			if (a == 1)
				return b;
			if (b == 1)
				return a;
			if (a == -1)
				return -b;
			if (b == -1)
				return -a;
			#endregion

			#region Initialize the adder and the result
			UInt64 adder = a._number;
			adder *= b._number;

			BigInt result = (UInt32) (adder & 0xFFFFFFFF);
			result._isNegative = a._isNegative != b._isNegative;
			adder = adder >> 32;
			if (a.Num32BitDigits > 0 || b.Num32BitDigits > 0 || adder != 0)
				result._32bitdigits = new List<UInt32>();
			#endregion

			#region result._32bitdigits = a._number * b._32bitdigits
			if (a._number > 0)
			{
				for (int i = 0; i < b.Num32BitDigits; ++i)
				{
					adder += (UInt64) (b._32bitdigits[i]) * (UInt64) (a._number);
					result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
					adder = adder >> 32;
				}
				if (adder > 0)
				{
					result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
					adder = adder >> 32;
				}
			}
			#endregion

			#region result._32bitdigits = a._32bitdigits * b._number
			if (b._number > 0)
			{
				for (int i = 0; i < a.Num32BitDigits; ++i)
				{
					adder += (UInt64) (a._32bitdigits[i]) * (UInt64) (b._number);
					if (i < result.Num32BitDigits)
					{
						adder += result._32bitdigits[i];
						result._32bitdigits[i] = (UInt32) (adder & 0xFFFFFFFF);
						adder = adder >> 32;
					}
					else
					{
						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
				}
				if (adder > 0)
				{
					result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
					adder = adder >> 32;
				}
			}
			#endregion

			#region result._32bitdigits += a._32bitdigits * b._32bitdigits
			for (int i = 0; i < a.Num32BitDigits; ++i)
			{
				int index;
				for (int j = 0; j < b.Num32BitDigits; ++j)
				{
					index = i + j;
					adder += (UInt64) (a._32bitdigits[i]) * (UInt64) (b._32bitdigits[j]);
					if (index < result.Num32BitDigits)
					{
						adder += result._32bitdigits[index];
						result._32bitdigits[index] = (UInt32) (adder & 0xFFFFFFFF);
						adder = adder >> 32;
					}
					else
					{
						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
				}
				index = i + b.Num32BitDigits;
				while (adder > 0)
				{
					if (index < result.Num32BitDigits)
					{
						result._32bitdigits[index] = (UInt32) (adder & 0xFFFFFFFF);
						adder = adder >> 32;
					}
					else
					{
						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
				}
			}
			#endregion

			return result;
		}

		public static BigInt operator +(BigInt a, BigInt b)
		{
			if (a._isNegative ^ b._isNegative)
			{
				b._isNegative = !b._isNegative;
				return a - b;
			}
			else
			{
				UInt64 adder;
				adder = a._number;
				adder += b._number;

				BigInt result = new BigInt((UInt32) (adder & 0xFFFFFFFF));
				result._isNegative = a._isNegative;
				adder = adder >> 32;

				if (adder > 0 || (a.Num32BitDigits > 0 && b.Num32BitDigits > 0))
				{
					result._32bitdigits = new List<UInt32>();
					int index;
					while ((index = result.Num32BitDigits) < a.Num32BitDigits &&
							index < b.Num32BitDigits)
					{
						adder += a._32bitdigits[index];
						adder += b._32bitdigits[index];

						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
					while ((index = result.Num32BitDigits) < a.Num32BitDigits && adder > 0)
					{
						adder += a._32bitdigits[index];

						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
					while ((index = result.Num32BitDigits) < a.Num32BitDigits)
					{
						result._32bitdigits.Add(a._32bitdigits[index]);
					}
					while ((index = result.Num32BitDigits) < b.Num32BitDigits && adder > 0)
					{
						adder += b._32bitdigits[index];

						result._32bitdigits.Add((UInt32) (adder & 0xFFFFFFFF));
						adder = adder >> 32;
					}
					while ((index = result.Num32BitDigits) < b.Num32BitDigits)
					{
						result._32bitdigits.Add(b._32bitdigits[index]);
					}
				}
				else if (a.Num32BitDigits != 0)
				{
					result._32bitdigits = a._32bitdigits;
				}
				else if (b.Num32BitDigits != 0)
				{
					result._32bitdigits = b._32bitdigits;
				}

				return result;
			}
		}

		public static BigInt operator -(BigInt a, BigInt b)
		{
			#region Check identities.
			if (a == b)
				return 0;

			if (a._isNegative ^ b._isNegative)
			{
				b._isNegative = !b._isNegative;
				return a + b;
			}
			#endregion

			#region Check for small magnitudeNum numbers
			BigInt result = 0;
			if (a.Num32BitDigits == 0 && b.Num32BitDigits == 0)
			{
				if (a._number > b._number)
				{
					result._isNegative = a._isNegative;
					result._number = a._number - b._number;
				}
				else
				{
					result._isNegative = !a._isNegative;
					result._number = b._number - a._number;
				}
				return result;
			}
			else if (a.Num32BitDigits == 0 || b.Num32BitDigits > a.Num32BitDigits)
			{
				a._isNegative = !a._isNegative;
				b._isNegative = !b._isNegative;
				return b - a;
			}
			#endregion

			#region Find the approximate magnitudeNum of the result.
			int size = 0;
			if (a.Num32BitDigits == b.Num32BitDigits)
			{
				for (int i = a.Num32BitDigits - 1; i >= 0; --i)
				{
					if (a._32bitdigits[i] > b._32bitdigits[i])
					{
						size = i + 1;
						break;
					}
					else if (b._32bitdigits[i] > a._32bitdigits[i])
					{
						a._isNegative = !a._isNegative;
						b._isNegative = !b._isNegative;
						return b - a;
					}
				}
			}
			#endregion

			#region If the result's magnitudeNum is small, calculate it.
			if (size == 0)
			{
				if (a._number > b._number)
				{
					result._isNegative = a._isNegative;
					result._number = a._number - b._number;
				}
				else
				{
					result._isNegative = !a._isNegative;
					result._number = b._number - a._number;
				}
				return result;
			}
			#endregion

			#region If result's magnitudeNum is bigger, calculate the result.
			result._32bitdigits = new List<uint>(size);
			result._isNegative = a._isNegative;

			uint carry = 0;
			for (int i = 0; i < size; ++i)
			{
				if (a._32bitdigits[i] >= b._32bitdigits[i])
				{
					result._32bitdigits.Add(a._32bitdigits[i] - b._32bitdigits[i]);
					if (result._32bitdigits[i] >= carry)
					{
						result._32bitdigits[i] -= carry;
						carry = 0;
					}
					else
					{
						result._32bitdigits[i] = uint.MaxValue - carry + 1 + result._32bitdigits[i];
						carry = 1;
					}
				}
				else
				{
					result._32bitdigits.Add(uint.MaxValue - b._32bitdigits[i]);
					result._32bitdigits[i] += a._32bitdigits[i];
					if (result._32bitdigits[i] >= carry)
					{
						result._32bitdigits[i] -= carry;
						carry = 1;
					}
					else
					{
						result._32bitdigits[i] = uint.MaxValue - carry + 1 + result._32bitdigits[i];
						carry = 2;
					}
				}
			}
			#endregion

			#region Trim the excess uint(s), if the magnitudeNum approximation was wrong.
			for (int i = result.Num32BitDigits - 1; i >= 0 && result._32bitdigits[i] == 0; --i)
				result._32bitdigits.RemoveAt(i);

			if (result.Num32BitDigits == 0)
				result._32bitdigits = null;
			else
				result._32bitdigits.TrimExcess();

			return result;
			#endregion
		}

		public static BigInt operator /(BigInt a, BigInt b)
		{
			return DoDivision(a, b);
		}

		public static BigInt operator %(BigInt a, BigInt b)
		{
			return DoDivision(a, b, true);
		}

		public static BigInt operator <<(BigInt a, int b)
		{
			if (b < 0)
				return a >> -b;
			if (b == 0)
				return a;

			int lowShift = b % 32;
			int highShift = 32 - lowShift;

			int rank = b / 32;

			UInt32 bits = a._number << lowShift;
			BigInt result = 0;

			if (b >= 32 || a.Num32BitDigits > 0 || ((a._number >> highShift) != 0))
				result._32bitdigits = new List<UInt32>();

			if (rank < 1)
			{
				result._number = bits;
			}
			else
			{
				for (int i = 1; i < rank; i++)
					result._32bitdigits.Add(0);
				result._32bitdigits.Add(bits);
			}

			if (b >= 32 || a.Num32BitDigits > 0 || ((a._number >> highShift) != 0))
				bits = a._number >> highShift;

			for (int i = 0; i < a.Num32BitDigits; i++)
			{
				bits |= (a._32bitdigits[i] << lowShift);

				if (i + 1 < a.Num32BitDigits || bits != 0)
					result._32bitdigits.Add(bits);

				bits = (a._32bitdigits[i] >> highShift);
			}

			if (bits != 0)
				result._32bitdigits.Add(bits);

			return result;
		}

		public static BigInt operator >>(BigInt a, int b)
		{
			if (b < 0)
				return a << -b;
			if (b == 0)
				return a;

			int lowShift = b % 32;
			int highShift = 32 - lowShift;

			int lowRank = b / 32 - 1;
			int highRank = lowRank + 1;

			UInt32 bits = 0;
			if (lowRank < 0)
				bits = (a._number & 0xFFFFFFFF) >> lowShift;
			else
				bits = (a._32bitdigits[lowRank] & 0xFFFFFFFF) >> lowShift;

			if ((highShift != 32) && (highRank < a.Num32BitDigits))
				bits |= (a._32bitdigits[highRank] << highShift) & 0xFFFFFFFF;

			BigInt result = bits;

			if (highRank < a.Num32BitDigits || ((lowRank < a.Num32BitDigits) && ((a._32bitdigits.Last() >> lowShift) != 0)))
				result._32bitdigits = new List<UInt32>();

			for (int i = 1; i + lowRank < a.Num32BitDigits; i++)
			{
				bits = (a._32bitdigits[lowRank + i] >> lowShift);
				if ((highShift != 32) && (highRank + i < a.Num32BitDigits))
					bits |= (a._32bitdigits[highRank] << highShift) & 0xFFFFFFFF;

				if (highRank + i < a.Num32BitDigits || bits != 0)
					result._32bitdigits.Add(bits);
			}

			return result;
		}

		public UInt64 SumDigits()
		{
			BigInt remainder = this;
			if (this._isNegative)
				remainder = -this;
			UInt64 sum = 0;

			while (remainder != 0)
			{
				sum += (remainder % 10)._number;
				remainder /= 10;
			}

			return sum;
		}
	}
}
