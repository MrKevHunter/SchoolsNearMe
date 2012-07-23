using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Embedded;

namespace SchoolMap.Net
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        private const string RavenSessionKey = "RavenMVC.Session";

        private static EmbeddableDocumentStore _store;

        public WebApiApplication()
        {
            BeginRequest += (sender, args) =>
                            HttpContext.Current.Items[RavenSessionKey] = _store.OpenSession();
            //Destroy the DocumentSession on EndRequest
            EndRequest += (o, eventArgs) =>
                              {
                                  var disposable = HttpContext.Current.Items[RavenSessionKey] as IDisposable;
                                  if (disposable != null)
                                      disposable.Dispose();
                              };
        }

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession) HttpContext.Current.Items[RavenSessionKey]; }
        }

        protected void Application_Start()
        {
            // to plumb in mvc profiler when we get there
            //http: //ayende.com/blog/38913/ravendb-mvc-profiler-support

            _store = new EmbeddableDocumentStore
                         {
                             DataDirectory = @"C:\coding\SchoolMap.Net\SchoolMap.Net\App_Data\RavenDb"
                         };
            _store.Initialize();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}