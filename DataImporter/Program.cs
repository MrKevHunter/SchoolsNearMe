using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using SchoolMap.Net.DataImporter;
using SchoolMap.Net.DataImporter.Commands;
using SchoolMap.Net.DataImporter.Indexes;
using SchoolMap.Net.Models;

namespace DataImporter
{
    internal class Program
    {
        private const string GeocodePath = "C:\\temp\\geocode.xml";
        private const string _dataDirectory = "../../../RavenDb";
        private static EmbeddableDocumentStore _store;

        private static void Main(string[] args)
        {
            _store = new EmbeddableDocumentStore
                         {
                             DataDirectory = _dataDirectory
                         };
            _store.Initialize();
            IndexCreation.CreateIndexes(typeof(FindSchoolByName).Assembly, _store);
          //  QueryDb(_store);
            //new ImportOfstedData().Execute(_store);

            new GetUnlocatedSchools2().Execute(_store);
            new UpdateUnlocatedSchoolsCommand().Execute(_store);
            new GetUnlocatedSchools2().Execute(_store);
            _store.Dispose();
            //  QueryDb();
            // ImportDataFromCsv();
       //    UpdateSpatial();
        //    GetUnlocatedSchools();
       //     GeocodeWithBing();

            Console.ReadLine();
        }

        private static void QueryDb(EmbeddableDocumentStore store)
        {
            IDocumentSession documentSession = store.OpenSession();
            var schools = documentSession.Query<School>().Where(x => x.SchoolName == "Pangbourne Primary School").ToList();
            foreach (var school in schools)
            {
                Console.WriteLine(school);
            }

   

        }
    }
}
