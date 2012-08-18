// Based on original by Tobi: https://gist.github.com/617852830394aaaa7160

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Extensions;
using Raven.Abstractions.Indexing;
using Raven.Client.Embedded;
using Raven.Database;
using Raven.Imports.Newtonsoft.Json;
using Raven.Json.Linq;

namespace SchoolsNearMe.Models
{
    public class RavenDbDumper
    {
        #region Static Methods

        private static void Read(JsonReader jsonReader, string arrayName, Action<RavenJToken> process)
        {
            if (jsonReader.Read() == false)
                return;
            if (jsonReader.TokenType != JsonToken.PropertyName)
                throw new InvalidDataException("PropertyName was expected");
            if (Equals(arrayName, jsonReader.Value) == false)
                throw new InvalidDataException(arrayName + " property was expected");
            if (jsonReader.Read() == false)
                return;
            if (jsonReader.TokenType != JsonToken.StartArray)
                throw new InvalidDataException("StartArray was expected");

            while (jsonReader.Read() && jsonReader.TokenType != JsonToken.EndArray)
            {
                var token = RavenJToken.ReadFrom(jsonReader);
                process(token);
            }
        }

        private static void WriteItemsFromDb(JsonWriter jsonWriter, string name, Func<int, RavenJArray> getBatchOfItems)
        {
            jsonWriter.WritePropertyName(name);
            jsonWriter.WriteStartArray();
            var totalCount = 0;
            while (true)
            {
                var array = getBatchOfItems(totalCount);
                if (array.Length == 0)
                {
                    break;
                }
                totalCount += array.Length;
                foreach (var item in array)
                {
                    item.WriteTo(jsonWriter);
                }
            }
            jsonWriter.WriteEndArray();
        }

        #endregion

        #region Constructors

        public RavenDbDumper(EmbeddableDocumentStore store)
        {
            _store = store;
            _documentDatabase = store.DocumentDatabase;
        }

        #endregion

        #region Fields

        private readonly DocumentDatabase _documentDatabase;
        private readonly EmbeddableDocumentStore _store;

        #endregion

        #region Methods

        public void Export(string file)
        {
            using (var streamWriter =
                new StreamWriter(new GZipStream(File.Create(file), CompressionMode.Compress)))
            {
                var jsonWriter = new JsonTextWriter(streamWriter)
                    {
                        Formatting = Formatting.Indented
                    };

                jsonWriter.WriteStartObject();
                WriteItemsFromDb(jsonWriter, "Indexes", start => _documentDatabase.GetIndexes(start, 128));
                WriteItemsFromDb(jsonWriter, "Docs", start => _documentDatabase.GetDocuments(start, 128, null));
                WriteItemsFromDb(jsonWriter, "Attachments", GetAttachments);

                jsonWriter.WriteEndObject();
            }
        }

        public DumperStats Import(string file)
        {
            var stopwatch = Stopwatch.StartNew();

            using (var streamReader =
                new StreamReader(new GZipStream(File.OpenRead(file), CompressionMode.Decompress)))
            {
                var jsonReader = new JsonTextReader(streamReader);
                if (jsonReader.Read() == false)
                    return new DumperStats();
                if (jsonReader.TokenType != JsonToken.StartObject)
                    throw new InvalidDataException("StartObject was expected");

                var indexes = new Dictionary<string, IndexDefinition>();
                var indexCount = 0;
                Read(jsonReader,
                     "Indexes",
                     index =>
                         {
                             indexCount++;
                             var indexName = index.Value<string>("name");
                             if (!indexName.StartsWith("Raven/"))
                             {
                                 indexes[indexName] = index.Value<RavenJObject>("definition").JsonDeserialization<IndexDefinition>();
                             }
                         });

                foreach (var index in indexes)
                {
                    _documentDatabase.PutIndex(index.Key, index.Value);
                }

                var total = 0;
                var batch = new List<RavenJObject>();
                Read(jsonReader,
                     "Docs",
                     document =>
                         {
                             total++;
                             batch.Add((RavenJObject)document);
                             if (batch.Count >= 128)
                             {
                                 FlushBatch(batch);
                             }
                         });
                FlushBatch(batch);

    

                stopwatch.Stop();
                return new DumperStats
                    {
                        Attachments = 0,
                        Documents = total,
                        Indexes = indexCount,
                        Elapsed = stopwatch.Elapsed
                    };
            }
        }

        private void FlushBatch(List<RavenJObject> batch)
        {
            _documentDatabase.Batch(batch.Select(x =>
                {
                    var metadata = x.Value<RavenJObject>("@metadata");
                    var key = metadata.Value<string>("@id");
                    x.Remove("@metadata");
                    return new PutCommandData
                        {
                            Document = x,
                            Etag = null,
                            Key = key,
                            Metadata = metadata
                        };
                }).ToArray());

            batch.Clear();
        }

        private RavenJArray GetAttachments(int start)
        {
            var array = new RavenJArray();
            var attachmentInfos = _documentDatabase.GetAttachments(start, 128, null,null,50000);

            foreach (var attachmentInfo in attachmentInfos)
            {
                var attachment = _store.DatabaseCommands.GetAttachment(attachmentInfo.Key);
                var bytes = StreamToBytes(attachment.Data());

                // Based on Raven.Smuggler.Api.ExportAttachments from build 888.
                array.Add(
                    new RavenJObject
                        {
                            {"Data", bytes},
                            {"Metadata", attachmentInfo.Metadata},
                            {"Key", attachmentInfo.Key}
                        });
            }
            return array;
        }

        /// <summary>
        ///   http://stackoverflow.com/a/221941/2608
        /// </summary>
        private byte[] StreamToBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

        #region AttachmentExportInfo

        private class AttachmentExportInfo
        {
            #region Properties

            public byte[] Data
            {
                get;
                set;
            }
            public string Key
            {
                get;
                set;
            }
            public RavenJObject Metadata
            {
                get;
                set;
            }

            #endregion
        }

        #endregion
    }
}