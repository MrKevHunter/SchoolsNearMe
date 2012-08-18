using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using SchoolMap.Net.Models.Indexes;

namespace SchoolMap.Net
{
    public class WebApiApplication : HttpApplication
    {
        private const string RavenSessionKey = "RavenMVC.Session";

        public static IDocumentStore Store;

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession) HttpContext.Current.Items[RavenSessionKey]; }
        }

        protected void Application_OnError()
        {
            Response.TrySkipIisCustomErrors = true;


        }

        protected void Application_Start()
        {
            Store = new DocumentStore()
            {
                ConnectionStringName = "RavenDb"
            };
             
            Store.Initialize();

            IndexCreation.CreateIndexes(typeof(FindSchoolByName).Assembly, Store);
            IndexCreation.CreateIndexes(typeof(FindSchoolByCoordsAndOfsted).Assembly, Store);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}