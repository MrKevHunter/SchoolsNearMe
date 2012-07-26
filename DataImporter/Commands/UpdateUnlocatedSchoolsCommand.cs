using System;
using System.Threading;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter.Commands
{
    internal class UpdateUnlocatedSchoolsCommand : IRavenCommand
    {

        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            new RavenEach().RavenForEach<School>(store, UpdateUnlocatedSchools);
        }

        #endregion

        private void UpdateUnlocatedSchools(School school)
        {
            if (school.Location.NotSet() && !string.IsNullOrWhiteSpace(school.PostCode))
            {
                SetCoords(school);
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
            catch(FormatException e)
            {
                Console.WriteLine("Unable to get address for {0}\nThere was a format exception", school);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to get address for {0}\nError was{1}", school, e);
            }
        }
    }
}