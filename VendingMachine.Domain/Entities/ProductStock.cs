using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Domain.Entities
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
			return Stock.Keys.First(p => p.Title == title);
		}

		public Product SellProduct(Product p)
		{
			if (!Stock.ContainsKey(p) || Stock[p] <= 0)
				throw new KeyNotFoundException();
			--Stock[p];

			return p;
		}
	}
}