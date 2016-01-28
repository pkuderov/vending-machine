using System;

namespace VendingMachine.Domain.Exceptions
{
	public class ProductIsNotInAssortmentException : Exception
	{
		public ProductIsNotInAssortmentException(string title)
			: base(string.Format("Product '{0}' is not in assortment", title))
		{ }
	}
}