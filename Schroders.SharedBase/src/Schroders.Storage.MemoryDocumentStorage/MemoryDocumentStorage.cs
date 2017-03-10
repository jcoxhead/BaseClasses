namespace Schroders.Storage.MemoryDocumentStorage
{
    using Core;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;


    public class MemoryDocumentStorage : IDocumentStorage
    {
        private readonly ConcurrentDictionary<string, DocumentCollection<object>> innerStorage = new ConcurrentDictionary<string, DocumentCollection<object>>();

        public void StoreDocument<TDocument>(string collectionName, string id, TDocument document)
        {
            var collection = this.innerStorage.GetOrAdd(collectionName, new DocumentCollection<object>());
            var documentCopy = document.CloneJson();
            collection[id] = documentCopy;
        }

        public IEnumerable<TDocument> GetAllDocuments<TDocument>(string collectionName)
        {
            DocumentCollection<object> collection;
            return !this.innerStorage.TryGetValue(collectionName, out collection) ? new TDocument[] { } : collection.Values.OfType<TDocument>().Select(d => d.CloneJson());
        }

        public TDocument GetDocument<TDocument>(string collectionName, string id)
        {
            DocumentCollection<object> collection;
            if (!this.innerStorage.TryGetValue(collectionName, out collection) || !collection.ContainsKey(id))
            {
                return default(TDocument);
            }

            return ((TDocument)collection[id]).CloneJson();
        }

        public void DeleteDocument<TDocument>(string collectionName, string id)
        {
            var collection = this.innerStorage.GetOrAdd(collectionName, new DocumentCollection<object>());
            collection.Remove(id);
        }
    }
}