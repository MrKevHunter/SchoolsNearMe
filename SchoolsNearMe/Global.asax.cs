using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SchoolsNearMe.App_Start;
using SchoolsNearMe.Attributes;

namespace SchoolsNearMe
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static Exception LastException;
        private const string RavenSessionKey = "RavenMVC.Session";

        public static IDocumentStore Store;

        public static IDocumentSession CurrentSession
        {
            get
            {
                return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey];
            }
        }

        protected void Application_OnError()
        {


        }

        protected void Application_Start()
        {

            try
            {
                
                Store = new DocumentStore(){ConnectionStringName = "RavenDb"};
                Store.Initialize();
/*                Store = new EmbeddableDocumentStore()
                {
                    DataDirectory = Server.MapPath("App_Data/Raven")
                };*/

                Store.Initialize();
            }
            catch (Exception e)
            {
                LastException = e;
                Console.WriteLine(e);
            }
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Filters.Add(new ElmahErrorAttribute());
        }
    }
}