using Microsoft.Practices.Unity;
using VendingMachine.Domain;

namespace VendingMachine.Persistence
{
	public static class DependencyResolver
	{
		public static IUnityContainer BuildContainer()
		{
			var vendingMachine = VendingMachineFactory.Build();
			var user = UserFactory.Build();

			var container = new UnityContainer();
			
			container.RegisterType<Domain.VendingMachine>(new ContainerControlledLifetimeManager()).RegisterInstance(vendingMachine);
			container.RegisterType<User>(new ContainerControlledLifetimeManager()).RegisterInstance(user);

			return container;
		}
	}
}
