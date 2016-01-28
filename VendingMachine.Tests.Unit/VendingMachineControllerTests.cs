using System;
using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using VendingMachine.Domain;
using VendingMachine.Domain.Exceptions;
using VendingMachine.Persistence;
using VendingMachine.Presentation.WebApi.Controllers;

namespace VendingMachine.Tests.Unit
{
	[TestFixture]
    public class VendingMachineControllerTests
	{
		private VendingMachineController _controller;

		[SetUp]
		public void SetUp()
		{
			var container = DependencyResolver.BuildContainer();
			container.RegisterType<VendingMachineController>(new ContainerControlledLifetimeManager());
			_controller = container.Resolve<VendingMachineController>();
		}

		[Test]
		public void Given_TwoRublesByOne_Then_ChangeInOneTwoRubleCoin()
		{
			var coin1 = new Coin(1);

			var sum = TotalSum();
			var totalCoins1 = TotalCoins(1);

			_controller.PutCoin(coin1);
			_controller.PutCoin(coin1);

			var change = _controller.ReturnChange().ToList();

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

			var userAmount = _controller.UserCoins().Amount();
			var sum = TotalSum();
			var totalCoins10 = TotalCoins(10);

			_controller.PutCoin(coin10);
			_controller.PutCoin(coin10);

			Assert.Throws<ProductIsNotInAssortmentException>(() => _controller.BuyProduct("RRR"));
			Assert.Throws<NotEnoughMoneyException>(() => _controller.BuyProduct("Кофе с молоком"));

			const string teaTitle = "Чай";
			var product = _controller.BuyProduct(teaTitle);
			Assert.AreEqual(product.Title, teaTitle);

			Assert.AreEqual(_controller.PaidAmount(), 7);
			var change = _controller.ReturnChange();

			Assert.AreEqual(change.Amount(), 7);
			Assert.AreEqual(sum, TotalSum());
			Assert.AreEqual(totalCoins10, TotalCoins(10));
			Assert.AreEqual(_controller.UserCoins().Amount(), userAmount - product.Cost);
		}


		private int TotalSum()
		{
			return _controller.Coins().Amount() + _controller.UserCoins().Amount();
		}

		private int TotalCoins(int value)
		{
			return _controller.Coins().Count(b => b.Value == value) + _controller.UserCoins().Count(b => b.Value == value);
		}
	}
}
