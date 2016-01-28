using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.Domain.Entities
{
	internal class CoinStock
	{
		public Dictionary<Coin, int> Stock { get; private set; }

		public CoinStock(IEnumerable<CoinBatch> stock)
		{
			Stock = stock.ToDictionary(batch => new Coin(batch.Value), batch => batch.Count);
		}

		public IEnumerable<CoinBatch> AsBatches
		{
			get { return Stock.Select(p => new CoinBatch(p.Key.Value, p.Value)); }
		}
		
		public void PutCoin(Coin coin)
		{
			if (!Stock.ContainsKey(coin))
				Stock[coin] = 1;
			else
				++Stock[coin];
		}

		public IReadOnlyCollection<CoinBatch> ReturnChangeFor(int sum)
		{
			var change = ReturnChangeFor(AsBatches, sum);
			Spend(change);
			return change;
		}

		public void Upsert(IEnumerable<CoinBatch> batches)
		{
			foreach (var batch in batches)
			{
				var coin = new Coin(batch.Value);
				if (!Stock.ContainsKey(coin))
					Stock[coin] = batch.Count;
				else
					Stock[coin] += batch.Count;

				if (Stock[coin] < 0)
					throw new NotEnoughCoinsException(coin);
			}
		}

		public void Spend(IEnumerable<CoinBatch> batches)
		{
			Upsert(batches.Select(b => new CoinBatch(b.Value, -b.Count)));
		}

		public IReadOnlyCollection<CoinBatch> SpendAll()
		{
			var result = AsBatches.ToList();
			Stock
				.Keys
				.ToList()
				.ForEach(coin => Stock[coin] = 0);

			return result;
		}

		private static IReadOnlyCollection<CoinBatch> ReturnChangeFor(IEnumerable<CoinBatch> stock, int sum)
		{
			var originalSum = sum;
			var change = new List<CoinBatch>();
			foreach (var batch in stock.OrderByDescending(cb => cb.Value))
			{
				if (sum == 0)
					break;

				var count = Math.Min(sum / batch.Value, batch.Count);
				if (count > 0)
				{
					var res = new CoinBatch(batch.Value, count);
					change.Add(res);
					sum -= res.Amount;
				}
			}
			if (sum != 0)
				throw new CannotReturnChangeException(originalSum);

			return change;
		}
	}
}