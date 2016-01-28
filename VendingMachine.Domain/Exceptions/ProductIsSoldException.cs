using System;

namespace VendingMachine.Domain.Exceptions
{
	public class ProductIsSoldException : Exception
	{
		public ProductIsSoldException(Product p)
			: base(string.Format("Product '{0}' is sold", p.Title))
		{ }
	}
}