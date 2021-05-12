using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NeighborHelpMobileClient.Utils
{
    internal class SslSertificateValidator
    {
        internal static HttpClientHandler BuildHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = CheckHttpPolicy
            };
            return handler;
        }

        private static bool CheckHttpPolicy(HttpRequestMessage sender, X509Certificate2 cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Validate the cert here and return true if it's correct.
            // If this is a development app, you could just return true always
            // In production you should ALWAYS either use a trusted cert or check the thumbprint of the cert matches one you expect.
            return true;
        }
    }
}
