namespace Schroders.Storage.Core
{
    using System.Collections.Generic;

    public interface IDocumentStorage
    {
        void StoreDocument<TDocument>(string collectionName, string id, TDocument document);

        IEnumerable<TDocument> GetAllDocuments<TDocument>(string collectionName);

        TDocument GetDocument<TDocument>(string collectionName, string id);

        void DeleteDocument<TDocument>(string collectionName, string id);
    }
}