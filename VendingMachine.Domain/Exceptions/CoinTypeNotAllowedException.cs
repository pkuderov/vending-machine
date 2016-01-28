using System;

namespace VendingMachine.Domain.Exceptions
{
	internal class CoinTypeNotAllowedException : Exception
	{
		public CoinTypeNotAllowedException(int value)
			:base(string.Format("Coins for {0} rubles aren't allowed", value))
		{ }
	}
}