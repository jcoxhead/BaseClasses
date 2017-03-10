using System.Collections.Generic;

namespace Schroders.ServiceBase.RestClient.Tests.Models
{
    public class ReadProduct
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public List<ProductAttribute> Attributes { get; set; }
    }
}
