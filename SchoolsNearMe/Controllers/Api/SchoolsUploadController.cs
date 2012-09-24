using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers.Api
{
    public class SchoolsUploadController : BaseApiController
    {
        // post: /api/schoolsupload
        public void Post(School school)
        {
            RavenSession.Store(school);
        }
    }
}