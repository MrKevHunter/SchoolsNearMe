using Raven.Client.Document;

namespace SchoolMap.Net.DataImporter
{
    public interface IRavenCommand
    {
        void Execute(DocumentStore store);
    }
}