using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter.Commands
{
    public class ImportOfstedData : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            const string importCsvPath = @"C:\coding\SchoolMap.Net\Collateral\Ofsted_Most_Recent_31_Mar_2012_(Provisional).csv";
                      
            var csvReader = new CsvReader(new StreamReader(File.OpenRead(importCsvPath)), true);
            Dictionary<string,OfstedRating> schoolRatings = new Dictionary<string, OfstedRating>();
            foreach (var line in csvReader)
            {
                OfstedRating rating =new OfstedRating();
                rating.LastInspection = DateTime.Parse(line[10]);
                rating.OverallEffectiveness = Convert.ToInt32(line[13]);
                rating.PupilAchievement = Convert.ToInt32(line[14]);
                rating.HowWellLearnerAchieve = Convert.ToInt32(line[15]);
                rating.PupilBehaviorAndSafety = Convert.ToInt32(line[16]);
                rating.QualityOfTeaching = Convert.ToInt32(line[17]);
                rating.LeaderhipAndManagement = Convert.ToInt32(line[18]);
                schoolRatings.Add(line[1], rating);

            }
            foreach (var ofstedRating in schoolRatings)
            {
                using (var session  = store.OpenSession())
                {
                    School school = session.Query<School>().SingleOrDefault(x => x.Id == ofstedRating.Key);
                    if (school != null) 
                    {
                        school.OfstedRating = ofstedRating.Value;
                        session.Store(school);
                        session.SaveChanges();
                    }
                }
            }

            Console.WriteLine(schoolRatings.Count);
        }
    }
}