using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers.Api
{
    public class SchoolsUploadController : BaseApiController
    {
        public void Post(School school)
        {
            RavenSession.Store(school);
        }
    }
}