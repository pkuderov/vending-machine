using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VendingMachine.Presentation.WebApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
			SetFormattersAndSerializers();
		}

		private static void SetFormattersAndSerializers()
		{
			GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
			var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
			formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}

	}
}
