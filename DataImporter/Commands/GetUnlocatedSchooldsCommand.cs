using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter
{
    class GetUnlocatedSchooldsCommand : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            int position = 0;
            const int batchSize = 120;
            bool recordsBeingReturned = true;
            int totalUnsetSchools = 0;
            while (recordsBeingReturned)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    IQueryable<School> schools = session.Query<School>().Skip(position).Take(batchSize);
                    recordsBeingReturned = schools.Any();
                    totalUnsetSchools += Enumerable.Count(schools, school => school.Location.NotSet());
                    position += batchSize;
                }
            }
            Console.WriteLine(totalUnsetSchools);
            
        }
    }
}