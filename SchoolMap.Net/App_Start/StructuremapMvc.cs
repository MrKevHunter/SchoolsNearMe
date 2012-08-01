using System.Web.Http;
using System.Web.Mvc;
using SchoolMap.Net.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SchoolMap.Net.App_Start.StructuremapMvc), "Start")]
namespace SchoolMap.Net.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = IoC.Initialize();
            var smDependencyResolver = new SmDependencyResolver(container);

            // Set the Api Resolver
            GlobalConfiguration.Configuration.DependencyResolver = smDependencyResolver;

            // Set the Web mvc Resolver
            DependencyResolver.SetResolver(smDependencyResolver);
        }
    }
}