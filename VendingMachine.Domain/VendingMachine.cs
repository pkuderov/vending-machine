using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.Domain
{
	public class VendingMachine
	{
		private readonly object _objLock;

		internal CoinStock Coins { get; private set; }
		internal CoinStock UserPaidCoins { get; private set; }
		internal ProductStock Products { get; private set; }

		public VendingMachine(IEnumerable<CoinBatch> coins, IEnumerable<CoinBatch> userPaidCoins, IEnumerable<ProductBatch> products)
		{
			_objLock = new object();

			Coins = new CoinStock(coins);
			UserPaidCoins = new CoinStock(userPaidCoins);
			Products = new ProductStock(products);
		}

		public IReadOnlyCollection<CoinBatch> GetCoins()
		{
			lock (_objLock)
			{
				return Coins.AsBatches.ToList();
			}
		}

		internal IReadOnlyCollection<CoinBatch> GetUserPaidCoins()
		{
			lock (_objLock)
			{
				return UserPaidCoins.AsBatches.ToList();
			}
		}
		
		public IReadOnlyCollection<ProductBatch> GetProducts()
		{
			lock (_objLock)
			{
				return Products.Stock.Select(pair => new ProductBatch(pair.Key, pair.Value)).ToList();
			}
		}

		public int PaidAmount
		{
			get
			{
				lock (_objLock)
				{
					return UserPaidCoins.AsBatches.Amount();
				}
			}
		}

		public void PutCoin(Coin coin)
		{
			ExecuteInTransation(() =>
			{
				lock (_objLock)
				{
					UserPaidCoins.PutCoin(coin);
				}
				return coin;
			});
		}

		public IReadOnlyCollection<CoinBatch> ReturnChange()
		{
			return ExecuteInTransation(() =>
			{
				lock (_objLock)
				{
					// change will be anyway
					var fromUserPaid = UserPaidCoins.SpendAll();
					Coins.Upsert(fromUserPaid);

					return Coins.ReturnChangeFor(fromUserPaid.Amount());
				}
			});
		}

		public Product BuyProduct(string title)
		{
			return ExecuteInTransation(() =>
			{
				lock (_objLock)
				{
					var product = Products.GetProduct(title);
					if (UserPaidCoins.AsBatches.Amount() < product.Cost)
						throw new NotEnoughMoneyException(product);

					// transfer all user paid money to VM cash
					var fromUserPaid = UserPaidCoins.SpendAll();
					Coins.Upsert(fromUserPaid);

					// try return change to user-paid cashier
					var change = Coins.ReturnChangeFor(fromUserPaid.Amount() - product.Cost);
					UserPaidCoins.Upsert(change);

					return Products.SellProduct(product);
				}
			});
		}

		private T ExecuteInTransation<T>(Func<T> transaction)
		{
			var memento = new VendingMachineMemento(this);
			try
			{
				return transaction();
			}
			catch (Exception)
			{
				Restore(memento);
				throw;
			}
		}

		private void Restore(VendingMachineMemento memento)
		{
			Coins = new CoinStock(memento.Coins);
			UserPaidCoins = new CoinStock(memento.UserPaidCoins);
			Products = new ProductStock(memento.Products);
		}
	}
}