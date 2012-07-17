using System;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using SchoolMap.Net.Models;

namespace DataImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryDb();
            //ImportData();
        }
/*
        private static void ImportData()
        {
            var store = new EmbeddableDocumentStore()
            {
                DataDirectory = "../../../App_Data/RavenDb"
            };
   
            store.Initialize();
            int i = 0;
            var reader = new StreamReader(File.OpenRead("C:\\coding\\schoolmap\\edubase.csv"));
            using (var session = store.OpenSession())
            {
                while (!reader.EndOfStream)
                {
                    Console.WriteLine(++i);

                    var items = reader.ReadLine().Split(',');


                    var school = new School
                                     {
                                         SchoolName = items[3].Trim('"'),
                                         PostCode = items[16].Trim('"'),
                                         Id = items[29].Trim('"')
                                     };
                    session.Store(school);

                }
                session.SaveChanges();
            }


            store.Dispose();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }  */
        
        
        private static void ImportData()
        {
            var store = new EmbeddableDocumentStore()
            {
                DataDirectory = "../../../App_Data/RavenDb"
            };
   
            store.Initialize();
            int i = 0;
            var csvReader = new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\schoolmap\\edubase.csv")), false);
  
            using (var session = store.OpenSession())
            {
                foreach(var line in csvReader)
                {
                    Console.WriteLine(++i);

                    var school = new School
                                     {
                                         SchoolName = line[3].Trim('"'),
                                         PostCode = line[16].Trim('"'),
                                         Id = line[29].Trim('"')
                                     };
                    session.Store(school);

                }
                session.SaveChanges();
            }


            store.Dispose();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        private static void QueryDb()
        {
            var store = new EmbeddableDocumentStore()
            {
                DataDirectory = "../../../App_Data/RavenDb"
            };

            store.Initialize();

            using (var session = store.OpenSession())
            {
                foreach (var item in session.Query<School>())
                {
                    Console.WriteLine(item.SchoolName);
                }
            }

            store.Dispose();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}
/* */