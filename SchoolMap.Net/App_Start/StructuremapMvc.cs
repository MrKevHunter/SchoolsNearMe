using System.Web.Http;
using SchoolMap.Net.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SchoolMap.Net.App_Start.StructuremapMvc), "Start")]

namespace SchoolMap.Net.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = IoC.Initialize();
            GlobalConfiguration.Configuration.DependencyResolver = new SmDependencyResolver(container);
        }
    }
}