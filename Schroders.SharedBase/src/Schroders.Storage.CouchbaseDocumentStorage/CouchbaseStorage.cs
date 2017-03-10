using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Schroders.Storage.Core;
using Schroders.Storage.CouchbaseDocumentStorage.Configurations;

namespace Schroders.Storage.CouchbaseDocumentStorage
{
    public class CouchbaseStorage : IDocumentStorage
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

            var query = $"select {bucket.Name}.* from {bucket.Name} where type = '{collectionName}'";
            var result = bucket.Query<TDocument>(query);
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

        private string GetDocumentId(string collectionName, string id)
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
