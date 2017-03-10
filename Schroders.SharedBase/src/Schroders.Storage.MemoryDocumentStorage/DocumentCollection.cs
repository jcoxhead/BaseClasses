namespace Schroders.Storage.MemoryDocumentStorage
{
    using System.Collections.Generic;

    public class DocumentCollection<TDocument> : Dictionary<string, TDocument>
    {
    }
}