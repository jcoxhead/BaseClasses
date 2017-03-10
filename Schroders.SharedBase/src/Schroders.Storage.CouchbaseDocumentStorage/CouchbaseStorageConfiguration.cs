using System.Collections.Generic;

namespace Schroders.Storage.CouchbaseDocumentStorage.Configurations
{
    public class CouchbaseStorageConfiguration
    {
        public List<string> ServerUrl { get; set; }
        public string BucketName { get; set; }
    }
}