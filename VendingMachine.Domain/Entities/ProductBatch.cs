namespace VendingMachine.Domain.Entities
{
	public class ProductBatch
	{
		public Product Product { get; private set; }
		public int Count { get; set; }

		public ProductBatch(Product product, int count)
		{
			Product = product;
			Count = count;
		}
	}
}