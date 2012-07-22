using System;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter
{
    class ImportFromCsvCommand : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            var csvReader = new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\schoolmap\\edubase.csv")), false);
            foreach (var line in csvReader)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    string id = line[29];
                    School school = session.Query<School>().SingleOrDefault(x => x.Id == id);
                    if (school == null)
                        school = new School
                                     {
                                         SchoolName = line[3],
                                         Street = line[23],
                                         Town = line[26],
                                         SchoolType = line[27],
                                         Website = line[30],
                                         PostCode = line[16],
                                         Id = id,
                                     };

                    session.Store(school);
                    session.SaveChanges();
                }
            }

            Console.WriteLine("Press any key to quit.");
        }
    }
}