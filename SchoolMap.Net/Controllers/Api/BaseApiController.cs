using System.Web.Http;
using Raven.Client;

namespace SchoolMap.Net.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        // todo: add in base bits to get RavenDB connection
        // http://msdn.microsoft.com/en-us/magazine/hh547101.aspx
        public IDocumentSession RavenSession { get; protected set; }
        
        public BaseApiController()
        {
            RavenSession = WebApiApplication.Store.OpenSession();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (RavenSession != null)
                {
                    using (RavenSession)
                    {
                        //if (this.AutoSave)
                        //    RavenSession.SaveChanges();
                        RavenSession.Dispose();
                        RavenSession = null;
                    }
                }
            }

            base.Dispose(disposing);
        } 
    }
}