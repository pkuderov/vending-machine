using System.Collections.Generic;

namespace VendingMachine.Domain
{
	internal class VendingMachineMemento
	{
		public IReadOnlyCollection<CoinBatch> Coins { get; private set; }
		public IReadOnlyCollection<CoinBatch> UserPaidCoins { get; private set; }
		public IReadOnlyCollection<ProductBatch> Products { get; private set; }

		public VendingMachineMemento(VendingMachine vm)
		{
			Coins = vm.GetCoins();
			UserPaidCoins = vm.GetUserPaidCoins();
			Products = vm.GetProducts();
		}
	}
}