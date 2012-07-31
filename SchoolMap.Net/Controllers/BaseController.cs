using System.Web.Mvc;
using Raven.Client;

namespace SchoolMap.Net.Controllers
{
    public class BaseController : Controller
    {
        // todo: add in base bits to get RavenDB connection
        // http://msdn.microsoft.com/en-us/magazine/hh547101.aspx
        public IDocumentSession RavenSession
        {
            get;
            protected set;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = WebApiApplication.Store.OpenSession();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            using (RavenSession)
            {
                if (filterContext.Exception != null)
                    return;

                if (RavenSession != null)
                    RavenSession.SaveChanges();
            }
        }
    }
}
