using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Domain
{
	public static class CoinBatchExtensions
	{
		public static int Amount(this IEnumerable<CoinBatch> stock)
		{
			return stock.Sum(b => b.Amount);
		}

	}
}