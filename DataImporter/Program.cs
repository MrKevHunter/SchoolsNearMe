using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using SchoolMap.Net.DataImporter.Commands;
using SchoolMap.Net.Models;
using SchoolMap.Net.Models.Indexes;

namespace DataImporter
{
    internal class Program
    {
        private const string GeocodePath = "C:\\temp\\geocode.xml";
        
        private static DocumentStore _store;

        private static void Main(string[] args)
        {
            _store = new DocumentStore
                         {
                             Url = "http://localhost:8080/"
                         };
            _store.Initialize();
            IndexCreation.CreateIndexes(typeof(FindSchoolByName).Assembly, _store);
          
            new ImportOfstedData().Execute(_store);

            _store.Dispose();


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
