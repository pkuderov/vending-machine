using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.Domain.Entities
{
	public class Coin : IComparable<Coin>, IEquatable<Coin>
	{
		private static readonly int[] AllowedValues = { 1, 2, 5, 10 };
		public static readonly List<Coin> AllowedCoins = AllowedValues.Select(value => new Coin(value)).ToList();

		public int Value { get; private set; }
		public Coin(int value)
		{
			Value = value;
			CheckCoinTypeExistence(value);
		}

		private static void CheckCoinTypeExistence(int value)
		{
			if (!AllowedValues.Contains(value))
				throw new CoinTypeNotAllowedException(value);
		}

		#region IComparable, etc.

		public int CompareTo(Coin other)
		{
			return Value.CompareTo(other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Coin && Equals((Coin) obj);
		}

		public bool Equals(Coin other)
		{
			return CompareTo(other) == 0;
		}

		public static bool operator ==(Coin x, Coin y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(Coin x, Coin y)
		{
			return !(x == y);
		}

		public override int GetHashCode()
		{
			return Value;
		}

		#endregion

	}
}
 