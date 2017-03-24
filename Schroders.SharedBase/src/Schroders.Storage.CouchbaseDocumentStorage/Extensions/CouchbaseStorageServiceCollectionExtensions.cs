using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schroders.Storage.Core;
using Schroders.Storage.CouchbaseDocumentStorage.Configurations;

namespace Schroders.Storage.CouchbaseDocumentStorage.Extensions
{
    public static class CouchbaseStorageServiceCollectionExtensions
    {
        public static void AddCouchbaseStorage(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IDocumentStorage, CouchbaseStorage>();

            var section = configuration.GetSection(nameof(CouchbaseStorageConfiguration));
            services.Configure<CouchbaseStorageConfiguration>(section);
        }
    }

}