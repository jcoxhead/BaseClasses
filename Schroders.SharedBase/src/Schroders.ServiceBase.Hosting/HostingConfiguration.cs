


namespace Schroders.ServiceBase.Hosting
{
    public class HostingConfiguration
    {
        public string ApplicationName { get; set; }

        public string BaseUrl { get; set; }

        public int HttpPort { get; set; }

        public int HttpsPort { get; set; }

        public bool IsSslEnabled { get; set; }

        public string CertificateFile { get; set; }

        public string CertificatePassword { get; set; }

        public string CertificateThumbprint { get; set; }

        public bool ForceHttpsRedirect { get; set; }
    }
}