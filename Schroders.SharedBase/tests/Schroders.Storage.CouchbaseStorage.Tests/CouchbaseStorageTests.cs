using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Options;
using Schroders.Storage.CouchbaseDocumentStorage.Configurations;

namespace Schroders.Storage.CouchbaseStorage.Tests
{
    public class Data
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public Data()
        {
        }

        public Data(string id)
        {
            Id = id;
            Code = $"Test Code {id}";
            Name = $"Test Name {id}";
        }
    }

    [Ignore]
    [TestClass]
    public class CouchbaseStorageTests
    {
        private const string BucketName = "TestCollection";

        [TestMethod]
        public void StoreDocument()
        {
            var optionsMock = new Mock<IOptions<CouchbaseStorageConfiguration>>();
            var couchbaseConfiguration = new CouchbaseStorageConfiguration { ServerUrl = new List<string> { "http://10.142.150.101" }, BucketName = BucketName };
            optionsMock.Setup(x => x.Value).Returns(couchbaseConfiguration);

            const string documentId = "TestCreate";

            var doc = new Data("TestCreateData");

            var storage = new CouchbaseDocumentStorage.CouchbaseStorage(optionsMock.Object);
            storage.StoreDocument(BucketName, documentId, doc);
        }

        [TestMethod]
        public void GetDocument()
        {
            var optionsMock = new Mock<IOptions<CouchbaseStorageConfiguration>>();
            var couchbaseConfiguration = new CouchbaseStorageConfiguration { ServerUrl = new List<string> { "http://10.142.150.101" }, BucketName = BucketName };
            optionsMock.Setup(x => x.Value).Returns(couchbaseConfiguration);

            var storage = new CouchbaseDocumentStorage.CouchbaseStorage(optionsMock.Object);

            var doc = new Data("TestGetData");

            const string documentId = "TestRead";

            storage.StoreDocument(BucketName, documentId, doc);

            var createdDoc = storage.GetDocument<Data>(BucketName, documentId);
            Assert.AreEqual(createdDoc.Id, doc.Id);
        }

        [TestMethod]
        public void GetDocuments()
        {
            var optionsMock = new Mock<IOptions<CouchbaseStorageConfiguration>>();
            var couchbaseConfiguration = new CouchbaseStorageConfiguration { ServerUrl = new List<string> { "http://10.142.150.101" }, BucketName = BucketName };
            optionsMock.Setup(x => x.Value).Returns(couchbaseConfiguration);

            var storage = new CouchbaseDocumentStorage.CouchbaseStorage(optionsMock.Object);

            storage.StoreDocument(BucketName, "TestRead1", new Data("TestGetData1"));
            storage.StoreDocument(BucketName, "TestRead2", new Data("TestGetData2"));

            var createdDocs = storage.GetAllDocuments<Data>(BucketName);
            Assert.IsTrue(createdDocs.Count() >= 2);
        }

        [TestMethod]
        public void DeleteDocument()
        {
            var optionsMock = new Mock<IOptions<CouchbaseStorageConfiguration>>();
            var couchbaseConfiguration = new CouchbaseStorageConfiguration { ServerUrl = new List<string> { "http://10.142.150.101" }, BucketName = BucketName };
            optionsMock.Setup(x => x.Value).Returns(couchbaseConfiguration);
            var storage = new CouchbaseDocumentStorage.CouchbaseStorage(optionsMock.Object);

            var doc = new Data("TestDeleteData");

            const string documentId = "TestDelete";

            storage.StoreDocument(BucketName, documentId, doc);
            var createdDoc = storage.GetDocument<Data>(BucketName, documentId);
            Assert.AreEqual(createdDoc.Id, doc.Id);

            storage.DeleteDocument<Data>(BucketName, "TestDelete");
            var deletedDoc = storage.GetDocument<Data>(BucketName, documentId);
            Assert.IsNull(deletedDoc);
        }
    }
}