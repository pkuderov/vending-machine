using System.Collections.Generic;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Domain
{
	internal class UserMemento
	{
		public IReadOnlyCollection<CoinBatch> Coins { get; private set; }

		public UserMemento(User user)
		{
			Coins = user.GetCoins();
		}
	}
}