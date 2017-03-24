

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Schroders.ServiceBase.Hosting.Helpers;

namespace Schroders.ServiceBase.Hosting.Extensions
{
    public static class KestrelServerOptionsExensions
    {
        public static void AddSharedKestrelOptions(this KestrelServerOptions options, HostingConfiguration config)
        {
            options.AddServerHeader = false;

            if (!config.IsSslEnabled)
            {
                return;
            }

            var certificate = CertificateHelper.GetCertificate(config.CertificateFile, config.CertificatePassword, config.CertificateThumbprint);

            options.UseHttps(certificate);

        }
    }
}
