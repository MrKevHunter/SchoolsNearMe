using System;
using System.Threading;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter.Commands
{
    internal class UpdateUnlocatedSchoolsCommand : IRavenCommand
    {
        private DocumentStore _store;

        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            _store = store;
            new RavenEach().RavenForEach<School>(store, UpdateUnlocatedSchools);
        }

        #endregion

        private void UpdateUnlocatedSchools(School school)
        {
            if (!school.Location.NotSet())
            {
                SetCoords(school);
                if (!school.Location.NotSet())
                {
                    using (IDocumentSession savingSession = _store.OpenSession())
                    {
                        savingSession.Store(school);
                        savingSession.SaveChanges();
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void SetCoords(School school)
        {
            try
            {
                GeocodeResult geocodeResult = Geocode.GetCoordinates(school.GetAddress());
                school.Location = geocodeResult.Location;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to get address for {0}\nError was{1}", school, e);
            }
        }
    }
}