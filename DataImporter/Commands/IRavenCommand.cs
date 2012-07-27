using Raven.Client.Document;

namespace SchoolMap.Net.DataImporter.Commands
{
    public interface IRavenCommand
    {
        void Execute(DocumentStore store);
    }
}