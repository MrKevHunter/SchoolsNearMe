using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using SchoolMap.Net.Models.Indexes;

namespace SchoolMap.Net
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        private const string RavenSessionKey = "RavenMVC.Session";

        public static IDocumentStore Store;



        public WebApiApplication()
        {
            
     }

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession) HttpContext.Current.Items[RavenSessionKey]; }
        }

        protected void Application_Start()
        {
            // to plumb in mvc profiler when we get there
            //http: //ayende.com/blog/38913/ravendb-mvc-profiler-support

            Store = new EmbeddableDocumentStore { ConnectionStringName = "RavenDB" };
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