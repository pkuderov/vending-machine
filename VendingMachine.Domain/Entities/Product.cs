using System;

namespace VendingMachine.Domain.Entities
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

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Product) obj);
		}

		public bool Equals(Product other)
		{
			return CompareTo(other) == 0;
		}

		public static bool operator ==(Product x, Product y)
		{
			return x != null && y != null && x.Title == y.Title;
		}

		public static bool operator !=(Product x, Product y)
		{
			return !(x == y);
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
		}

		#endregion
	}
}