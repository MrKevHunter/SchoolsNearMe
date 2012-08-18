using Raven.Client.Document;

namespace SchoolsNearMe.DataImporter.Commands
{
    public interface IRavenCommand
    {
        void Execute(DocumentStore store);
    }
}