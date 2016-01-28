using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.Domain
{
	internal class ProductStock
	{
		public Dictionary<Product, int> Stock { get; private set; }

		public ProductStock(IEnumerable<ProductBatch> stock)
		{
			Stock = stock.ToDictionary(b => b.Product, b => b.Count);
		}

		public Product GetProduct(string title)
		{
			var product = Stock.Keys.FirstOrDefault(p => p.Title == title);
			if (product == null)
				throw new ProductIsNotInAssortmentException(title);
			return product;
		}

		public Product SellProduct(Product p)
		{
			if (!Stock.ContainsKey(p))
				throw new ProductIsNotInAssortmentException(p.Title);
			if (Stock[p] <= 0)
				throw new ProductIsSoldException(p);
			--Stock[p];
			return p;
		}
	}
}