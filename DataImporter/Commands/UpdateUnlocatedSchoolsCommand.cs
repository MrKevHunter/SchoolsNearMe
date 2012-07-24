using System;
using System.Linq;
using System.Threading;
using DataImporter;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter
{
    class UpdateUnlocatedSchoolsCommand : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            int position = 0;
            const int batchSize = 120;
            bool recordsBeingReturned = true;
            int fails = 0;
            int successes = 0;
            while (recordsBeingReturned)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    IQueryable<School> schools = session.Query<School>().Skip(position).Take(batchSize);
                    recordsBeingReturned = schools.Any();
                    foreach (var school in schools)
                    {
                        if (!school.Location.NotSet()) continue;
                        
                        SetCoords(school);
                        if (school.Location.NotSet())
                        {
                            fails++;
                        }
                        else
                        {
                            successes++;
                            using (var savingSession = store.OpenSession())
                            {
                                savingSession.Store(school);
                                savingSession.SaveChanges();
                            }
                        }
                        Thread.Sleep(500);
                    }
                    position += batchSize;
                }
            }
            Console.WriteLine("{0} successes\n{1} fails", successes, fails);
        }

        private void SetCoords(School school)
        {
            try
            {
                var geocodeResult = Geocode.GetCoordinates(school.GetAddress());
                school.Location = geocodeResult.Location;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to get address for {0}\nError was{1}",school,e);
            }
        }
    }
}