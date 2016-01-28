using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Domain
{
	public class User
	{
		private readonly object _objLock;
		internal CoinStock Coins { get; private set; }

		public User(IEnumerable<CoinBatch> coins)
		{
			_objLock = new object();
			Coins = new CoinStock(coins);
		}

		public IReadOnlyCollection<CoinBatch> GetCoins()
		{
			lock (_objLock)
			{
				return Coins.AsBatches.ToList();
			}
		}

		public void SpendCoin(Coin coin)
		{
			ExecuteInTransation(() =>
			{
				lock (_objLock)
				{
					Coins.Spend(new[] { new CoinBatch(coin.Value, 1) });
				}
			});
		}

		public void TakeChange(IEnumerable<CoinBatch> change)
		{
			ExecuteInTransation(() =>
			{
				lock (_objLock)
				{
					Coins.Upsert(change);
				}
			});
		}

		private void ExecuteInTransation(Action transaction)
		{
			var memento = new UserMemento(this);
			try
			{
				transaction();
			}
			catch (Exception)
			{
				Restore(memento);
				throw;
			}
		}

		private void Restore(UserMemento memento)
		{
			Coins = new CoinStock(memento.Coins);
		}
	}
}