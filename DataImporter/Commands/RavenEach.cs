using System;
using System.Diagnostics;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace SchoolMap.Net.DataImporter.Commands
{
    public class RavenEach
    {
        public void RavenForEach<T>(DocumentStore store, Action<T> action)
        {
            var sw = Stopwatch.StartNew();
            int position = 0;
            const int batchSize = 120;
            bool recordsBeingReturned = true;
            while (recordsBeingReturned)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    IQueryable<T> items = session.Query<T>().Skip(position).Take(batchSize);
                    recordsBeingReturned = items.Any();
                    foreach (T item in items)
                    {
                        action(item);

                        using (IDocumentSession savingSession = store.OpenSession())
                        {
                            savingSession.Store(item);
                            savingSession.SaveChanges();
                        }
                    }
                    position += batchSize;
                }
            }
            sw.Stop();
            Console.WriteLine("Raven ForEach completed in {0} seconds", sw.Elapsed.TotalSeconds);
        }
    }
}