using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using ThirdPartyEventEditor.Services;
using Unity.Mvc4;

namespace ThirdPartyEventEditor
{
    /// <summary>
    /// Represent funcionality for dependency injection from controllers
    /// </summary>
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            string fileName = WebConfigurationManager.AppSettings["JsonSourcePath"];
            string path = System.Web.HttpContext.Current.Server.MapPath(fileName);
            container.RegisterType<IThirdPartyEventService, ThirdPartyEventService>(
                new InjectionConstructor(path));

            return container;
        }
    }
}