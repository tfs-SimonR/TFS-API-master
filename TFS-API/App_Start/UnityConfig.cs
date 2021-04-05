using System.Web.Http;
using TFS_API.Services.Interfaces;
using Unity;
using Unity.WebApi;

namespace TFS_API
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IHandyScannerService, IHandyScannerService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}