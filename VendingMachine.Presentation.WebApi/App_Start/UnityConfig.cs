using System.Web.Http;
using Unity.WebApi;
using VendingMachine.Persistence;

namespace VendingMachine.Presentation.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = DependencyResolver.BuildContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}