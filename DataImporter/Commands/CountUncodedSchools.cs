using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter.Commands
{
    public class CountUncodedSchools : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            IDocumentSession documentSession = store.OpenSession();
            int pos = 0;
            int batch = 1024;
            int total = 0;
            var schools =
                documentSession.Query<School>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(
                    x => x.Location.Latitude == 0).Skip(pos).Take(batch).ToList();
            int count = schools.Count;
            total = count;
            while (count > 0)
            {
                schools =
                    documentSession.Query<School>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(
                        x => x.Location.Latitude == 0).Skip(pos).Take(batch).ToList();
                pos += batch;
                count = schools.Count;
                total += count;
            }

            Console.WriteLine("{0} schools are uncoded");
            Console.ReadLine();
        }
    }
}