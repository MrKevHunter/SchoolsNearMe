using System.Linq;
using Raven.Client.Indexes;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter.Indexes
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