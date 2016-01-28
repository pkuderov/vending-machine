using System;

namespace VendingMachine.Domain.Exceptions
{
	public class NotEnoughMoneyException : Exception
	{
		public NotEnoughMoneyException(Product product)
			: base (string.Format("Not enough money to buy product '{0}' for {1} rubles", product.Title, product.Cost))
		{ }
	}
}