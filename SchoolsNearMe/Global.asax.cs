using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using System.Linq;
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
        private const string RavenDbUseEmbeddedKey = "ravendb:UseEmbedded";

        public static IDocumentStore Store;

        public static IDocumentSession CurrentSession
        {
            get
            {
                return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey];
            }
        }

        private bool UseEmbedded
        {
            get
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains(RavenDbUseEmbeddedKey))
                {
                    return bool.Parse(ConfigurationManager.AppSettings[RavenDbUseEmbeddedKey]);
                }
                return false;
            }
        }

        protected void Application_OnError()
        {


        }

        protected void Application_Start()
        {

            try
            {
                if (UseEmbedded)
                {
                    Store = new EmbeddableDocumentStore
                        {
                            DataDirectory = Server.MapPath("App_Data/Raven")
                        };
                }
                else
                {
                    Store = new DocumentStore
                        {
                            ConnectionStringName = "RavenDb"
                        };    
                }
                
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