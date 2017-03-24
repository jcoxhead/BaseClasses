
namespace Schroders.ServiceBase.Hosting.Extensions
{
    public static class HostingConfigurationExtensions
    {
        public static string GetUrls(this HostingConfiguration config)
        {
            var urls = GetUrl("http", config.BaseUrl, config.HttpPort.ToString());

            if (config.IsSslEnabled)
            {
                urls += ";";
                urls += GetUrl("https", config.BaseUrl, config.HttpsPort.ToString());
            }

            return urls;
        }

        private static string GetUrl(string protocol, string baseUrl, string port)
        {
            return $"{protocol}://{baseUrl}:{port}";
        }
    }
}