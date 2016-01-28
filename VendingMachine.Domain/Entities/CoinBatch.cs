namespace VendingMachine.Domain.Entities
{
	public class CoinBatch
	{
		public int Value { get; private set; }
		public int Count { get; set; }

		public CoinBatch(int value, int count)
		{
			Value = value;
			Count = count;
		}

		public int Amount
		{
			get { return Value*Count; }
		}
	}
}