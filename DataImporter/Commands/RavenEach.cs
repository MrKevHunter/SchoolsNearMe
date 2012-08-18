using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace SchoolsNearMe.DataImporter.Commands
{
    public class RavenEach
    {
        public void RavenForEach<T>(DocumentStore store, Action<T> action)
        {
            int counter = 0;
            var sw = Stopwatch.StartNew();
            int position = 0;
            const int batchSize = 1000;
            bool recordsBeingReturned = true;
            var batch = new List<T>();
            while (recordsBeingReturned)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    IQueryable<T> items = session.Query<T>().Skip(position).Take(batchSize);
                    recordsBeingReturned = items.Any();
                    foreach (T item in items)
                    {
                        Console.Write("\rProcessing Record :{0}          ", ++counter);
                        action(item);

                        if (batch.Count == 30)
                        {
                            var savingSession = store.OpenSession();
                            batch.ForEach(x => savingSession.Store(x));
                            savingSession.SaveChanges();
                            batch.Clear();
                        }
                        batch.Add(item);
                    }
                    position += batchSize;

                }
            }
            sw.Stop();
            Console.WriteLine("Raven ForEach completed in {0} seconds", sw.Elapsed.TotalSeconds);
        }
    }
}