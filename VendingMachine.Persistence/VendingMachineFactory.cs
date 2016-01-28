using VendingMachine.Domain;

namespace VendingMachine.Persistence
{
	public static class VendingMachineFactory
	{
		public static Domain.VendingMachine Create()
		{
			return new Domain.VendingMachine(
				new[]
				{
					new CoinBatch(1, 100),
					new CoinBatch(2, 100),
					new CoinBatch(5, 100), 
					new CoinBatch(10, 100), 
				},
				new CoinBatch[] { },
				new[]
				{
					new ProductBatch(new Product("���", 13), 10),
					new ProductBatch(new Product("����", 18), 20), 
					new ProductBatch(new Product("���� � �������", 21), 20),
					new ProductBatch(new Product("���", 35), 15)
				}
			);
		}
	}
}