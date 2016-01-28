using System;

namespace VendingMachine.Domain.Exceptions
{
	public class CannotReturnChangeException : Exception
	{
		public CannotReturnChangeException(int sum)
			: base(string.Format("Cannot find change for {0} rubles", sum))
		{ }
	}
}