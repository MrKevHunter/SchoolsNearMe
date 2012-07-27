using System.Linq;
using Raven.Client.Indexes;

namespace SchoolMap.Net.Models.Indexes
{
    public class FindSchoolByName : AbstractIndexCreationTask<School>
    {
        public FindSchoolByName()
        {
            Map = schools => from school in schools
                             select new {school.SchoolName};
        }
    }
}