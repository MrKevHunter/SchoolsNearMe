using System;
using Raven.Client.Document;
using SchoolsNearMe.DataImporter.Commands;

namespace SchoolsNearMe.DataImporter
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
          /*  IndexCreation.CreateIndexes(typeof(FindSchoolByName).Assembly, _store);
            new StopIndexing().Execute(_store);
            new ImportFromCsvCommand().Execute(_store);
            new StartIndexing().Execute(_store);
          */
            new CheckProximityCommand().Execute(_store);
            _store.Dispose();

            Console.ReadLine();
        }

  
    }
}
