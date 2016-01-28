using System;

namespace VendingMachine.Domain.Exceptions
{
	internal class NotEnoughCoinsException : Exception
	{
		public NotEnoughCoinsException(Coin coin)
			: base(string.Format("Not enough {0}-rubles coins", coin.Value))
		{ }
	}
}