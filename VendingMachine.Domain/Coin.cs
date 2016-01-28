using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.Domain
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

		public bool Equals(Coin other)
		{
			return other != null && CompareTo(other) == 0;
		}

		public override int GetHashCode()
		{
			return Value;
		}

		#endregion

	}
}
 