using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter
{
    class GeocodeWithBingCommand : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            throw new NotImplementedException();
   /*         var allSchools = new List<School>();
            int position = 0;
            const int batchSize = 120;
            bool recordsBeingReturned = true;
            using (IDocumentSession session = store.OpenSession())
            {
                IQueryable<School> schools = session.Query<School>().Skip(position).Take(batchSize);
                allSchools.AddRange(schools);
            }

            Console.WriteLine(allSchools.Count);
            return new BingXmlFormatter().CreateBingGeocodeXml(allSchools);*/
        }

        private static void GeocodeWithBing()
        {
            /*        string result = GenerateBingGeocode();
            File.WriteAllText(GeocodePath, result);
            new BingGeoCoder().GeocodeLocations(GeocodePath);*/
        }

        private static string GenerateBingGeocode()
        {
            return null;
        }


    }
}