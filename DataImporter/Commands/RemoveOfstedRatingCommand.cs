using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter.Commands
{
    public class RemoveOfstedRatingCommand : IRavenCommand
    {
        private DocumentStore _store;

        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            _store = store;
            new RavenEach().RavenForEach<School>(store, RemoveOfstedRating);
        }

        #endregion

        private void RemoveOfstedRating(School obj)
        {
            if (obj.OfstedRating == null)
            {
                return;
            }
            _store.DatabaseCommands.Patch(
                "schools/" + obj.Id,
                new[]
                    {
                        new PatchRequest
                            {
                                Type = PatchCommandType.Remove,
                                Name = "OfstedRating",
                                Value = RavenJObject.FromObject(obj.OfstedRating)
                            }
                    });
        }
    }
}