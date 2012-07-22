using System;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using SchoolMap.Net.Models;
using System.Linq;

namespace SchoolMap.Net.DataImporter.Commands
{
    public class GetUnlocatedSchools2 : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            using (IDocumentSession documentSession = store.OpenSession())
            {
                RavenQueryStatistics stats = null;

               var result = documentSession.Query<School>().Statistics(out stats).Where(x => x.Location.Latitude == 0M && x.Location.Longitude == 0M).
                    ToList();

                Console.WriteLine(stats.TotalResults);
            }
        }
    }
}