using System;
using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using VendingMachine.Domain;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Exceptions;
using VendingMachine.Persistence;

namespace VendingMachine.Tests.Unit
{
	[TestFixture]
    public class Interactions
	{
		private Domain.VendingMachine _vendingMachine;
		private User _user;

		[SetUp]
		public void SetUp()
		{
			var container = DependencyResolver.BuildContainer();
			_vendingMachine = container.Resolve<Domain.VendingMachine>();
			_user = container.Resolve<User>();
		}

		[Test]
		public void Given_TwoRublesByOne_Then_ChangeInOneTwoRubleCoin()
		{
			var coin1 = new Coin(1);

			var sum = TotalSum();
			var totalCoins1 = TotalCoins(1);

			_vendingMachine.PutCoin(coin1);
			_user.SpendCoin(coin1);

			_vendingMachine.PutCoin(coin1);
			_user.SpendCoin(coin1);

			var change = _vendingMachine.ReturnChange();
			_user.TakeChange(change);

			Assert.AreEqual(change.Count, 1);
			Assert.AreEqual(change.First().Amount, 2);
			Assert.AreEqual(change.First().Value, 2);

			Assert.AreEqual(sum, TotalSum());
			Assert.AreEqual(totalCoins1, TotalCoins(1));
		}

		[Test]
		public void Given_TwentyRubles_Then_NotEnoughMoneyToBuyCoffee_CanBuyTea_And_SevenRublesInChange()
		{
			var coin10 = new Coin(10);

			var userAmount = _user.GetCoins().Amount();
			var sum = TotalSum();
			var totalCoins10 = TotalCoins(10);

			_vendingMachine.PutCoin(coin10);
			_user.SpendCoin(coin10);

			_vendingMachine.PutCoin(coin10);
			_user.SpendCoin(coin10);

			Assert.Throws<InvalidOperationException>(() => _vendingMachine.BuyProduct("RRR"));
			Assert.Throws<NotEnoughMoneyException>(() => _vendingMachine.BuyProduct("Кофе с молоком"));

			const string teaTitle = "Чай";
			var product = _vendingMachine.BuyProduct(teaTitle);
			Assert.AreEqual(product.Title, teaTitle);

			Assert.AreEqual(_vendingMachine.PaidAmount, 7);
			var change = _vendingMachine.ReturnChange();
			_user.TakeChange(change);

			Assert.AreEqual(sum, TotalSum());
			Assert.AreEqual(totalCoins10, TotalCoins(10));
			Assert.AreEqual(_user.GetCoins().Amount(), userAmount - product.Cost);
		}


		private int TotalSum()
		{
			return _vendingMachine.GetCoins().Amount() + _user.GetCoins().Amount();
		}

		private int TotalCoins(int value)
		{
			return _vendingMachine.GetCoins().Count(b => b.Value == value) + _user.GetCoins().Count(b => b.Value == value);
		}
	}
}
