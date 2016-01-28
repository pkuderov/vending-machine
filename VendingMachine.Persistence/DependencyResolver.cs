using Microsoft.Practices.Unity;
using VendingMachine.Domain;

namespace VendingMachine.Persistence
{
	public static class DependencyResolver
	{
		public static IUnityContainer BuildContainer()
		{
			var vendingMachine = VendingMachineFactory.Create();
			var user = UserFactory.Create();

			var container = new UnityContainer();
			
			container.RegisterType<Domain.VendingMachine>(new ContainerControlledLifetimeManager()).RegisterInstance(vendingMachine);
			container.RegisterType<User>(new ContainerControlledLifetimeManager()).RegisterInstance(user);

			return container;
		}
	}
}
