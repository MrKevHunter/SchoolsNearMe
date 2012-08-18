using System.Web.Http;
using System.Web.Mvc;
using SchoolsNearMe.App_Start;
using SchoolsNearMe.DependencyResolution;

[assembly: WebActivator.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
namespace SchoolsNearMe.App_Start
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