using VendingMachine.Domain;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Persistence
{
	public static class UserFactory
	{
		public static User Build()
		{
			return new User(new[]
			{
				new CoinBatch(1, 10), 
				new CoinBatch(2, 30), 
				new CoinBatch(5, 20), 
				new CoinBatch(10, 15), 
			});
		}
	}
}