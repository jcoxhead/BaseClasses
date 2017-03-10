using System.Collections.Generic;

namespace Schroders.ServiceBase.RestClient.Tests.Models
{
    public class ProductAttribute
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TextValue { get; set; }

        public IDictionary<string, object> Fields { get; set; }
    }
}