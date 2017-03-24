using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.N1QL;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Schroders.Storage.Core;
using Schroders.Storage.CouchbaseDocumentStorage.Configurations;

namespace Schroders.Storage.CouchbaseDocumentStorage
{
    public class CouchbaseStorage : IDocumentStorage, IDisposable
    {
        private IBucket bucket;
        private readonly CouchbaseStorageConfiguration couchbaseStorageConfiguration;
        private readonly object thisLock = new object();

        public CouchbaseStorage(IOptions<CouchbaseStorageConfiguration> couchBaseSettings)
        {
            this.couchbaseStorageConfiguration = couchBaseSettings.Value;
        }

        public void StoreDocument<TDocument>(string collectionName, string id, TDocument document)
        {
            Initialize();

            var storageObject = JObject.FromObject(document);
            storageObject.Add("type", collectionName);

            var documentId = GetDocumentId(collectionName, id);
            var doc = new Document<JObject>
            {
                Id = documentId,
                Content = storageObject
            };

            var result = bucket.Upsert(doc);
            if (!result.Success)
            {
                throw new Exception(result.Message);
            }
        }

        public IEnumerable<TDocument> GetAllDocuments<TDocument>(string collectionName)
        {
            Initialize();

            var queryRequest = new QueryRequest()
                .Statement($"select {bucket.Name}.* from {bucket.Name} where type = $collectionName")
                .AddNamedParameter("collectionName", collectionName)
                .ScanConsistency(ScanConsistency.RequestPlus);

            var result = bucket.Query<TDocument>(queryRequest);
            return result.Rows;
        }

        public TDocument GetDocument<TDocument>(string collectionName, string id)
        {
            Initialize();

            var documentId = GetDocumentId(collectionName, id);

            var doc = bucket.GetDocument<TDocument>(documentId);
            if (doc == null)
            {
                return default(TDocument);
            }

            return doc.Content;
        }

        public void DeleteDocument<TDocument>(string collectionName, string id)
        {
            Initialize();

            var documentId = GetDocumentId(collectionName, id);
            bucket.Remove(documentId);
        }

        public void Dispose()
        {
            if (bucket != null)
            {
                bucket.Dispose();
                bucket = null;
            }
        }

        private static string GetDocumentId(string collectionName, string id)
        {
            return $"{collectionName}_{id}";
        }

        private void Initialize()
        {
            if (bucket != null)
            {
                return;
            }

            lock (thisLock)
            {
                if (bucket != null)
                {
                    return;
                }

                var config = new ClientConfiguration
                {
                    Servers = couchbaseStorageConfiguration.ServerUrl.Select(x => new Uri(x)).ToList()
                };

                var cluster = new Cluster(config);
                bucket = cluster.OpenBucket(couchbaseStorageConfiguration.BucketName);
            }
        }
    }
}