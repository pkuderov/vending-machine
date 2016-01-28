using System;

namespace VendingMachine.Domain
{
	public class Product : IComparable<Product>, IEquatable<Product>
	{
		/// <summary>
		/// Title represents unique id
		/// </summary>
		public string Title { get; private set; }
		public int Cost { get; private set; }

		public Product(string title, int cost)
		{
			Title = title;
			Cost = cost;
		}

		#region IComparable, etc.

		public int CompareTo(Product other)
		{
			return string.Compare(Title, other.Title, StringComparison.Ordinal);
		}

		public bool Equals(Product other)
		{
			return other != null && CompareTo(other) == 0;
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
		}

		#endregion
	}
}