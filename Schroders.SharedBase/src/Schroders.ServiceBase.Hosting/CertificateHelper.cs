using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Schroders.ServiceBase.Hosting.Helpers
{
    public static class CertificateHelper
    {
        public static X509Certificate2 GetCertificate(string filePath, string filePassword, string thumbprint)
        {
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(filePassword))
            {
                return new X509Certificate2(AppDomain.CurrentDomain.BaseDirectory + filePath, filePassword);
            }

            if (!string.IsNullOrEmpty(thumbprint))
            {
                thumbprint = Regex.Replace(thumbprint, @"[^\da-zA-z]", string.Empty).ToUpper();

                var certCollection = OpenCertificateCollection(thumbprint, StoreName.My, StoreLocation.CurrentUser);
                if (certCollection.Count == 0)
                {
                    certCollection = OpenCertificateCollection(thumbprint, StoreName.My, StoreLocation.LocalMachine);
                }

                if (certCollection.Count > 0)
                {
                    return certCollection[0];
                }
            }

            return null;
        }

        private static X509Certificate2Collection OpenCertificateCollection(string thumbprint, StoreName name, StoreLocation location)
        {
            var store = new X509Store(name, location);
            store.Open(OpenFlags.ReadOnly);
            var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            store.Close();
            return certCollection;
        }
    }
}

