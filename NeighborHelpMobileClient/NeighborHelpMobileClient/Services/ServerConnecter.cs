using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.Services
{
    public class ServerConnecter
    {
        private const string AuthorizationHeader = "Authorization";
        private double RequestTimeout;
        private bool MultithreadingMode;

        public ServerConnecter(bool supportMultithreading = DefaultSettings.SupportMultithreadingWebRequests, double requestTimeout = DefaultSettings.RequestTimeout)
        {
            MultithreadingMode = supportMultithreading;
            RequestTimeout = requestTimeout;
        }

        private IConnectorProvider ConnectionProvider = DependencyService.Get<IConnectorProvider>();

        private string HostAddress => ConnectionProvider.GetServerUrl();

        private string AuthorizationToken => ConnectionProvider.GetToken()?.Token;

        public async Task<string> Get(string request, bool useToken=false)
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            var response = await client.GetAsync(requestUri);

            var statusCode = response.StatusCode;
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<T> Get<T>(string request, bool useToken = false) where T:class
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            var response = await client.GetAsync(requestUri);

            var statusCode = response.StatusCode;
            var result = await response.Content?.ReadAsStringAsync();

            if (statusCode!= System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(result))
                return null;

            T resultObject = JsonConvert.DeserializeObject<T>(result);

            return resultObject;
        }

        public async Task<IEnumerable<T>> GetList<T>(string request, bool useToken = false)
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            var response = await client.GetAsync(requestUri);

            var statusCode = response.StatusCode;
            var result = await response.Content?.ReadAsStringAsync();

            if (statusCode != System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(result))
                return new List<T>();

            IEnumerable<T> resultList = JsonConvert.DeserializeObject<IEnumerable<T>>(result);

            return resultList;
        }

        public async Task<string> Post<T>(string request, T content, bool useToken = false) where T : class
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            string json = JsonConvert.SerializeObject(content);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, httpContent);

            var statusCode = response.StatusCode;
            var result = await response.Content?.ReadAsStringAsync();

            return result;
        }

        public async Task<string> Put<T>(string request, T content, bool useToken = false) where T : class
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            string json = JsonConvert.SerializeObject(content);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(requestUri, httpContent);

            var statusCode = response.StatusCode;
            var result = await response.Content?.ReadAsStringAsync();

            return result;
        }

        public async Task<string> Delete(string request, bool useToken = false)
        {
            var client = GetClient(useToken);
            var requestUri = BuildRequest(request);

            var response = await client.DeleteAsync(requestUri);

            var statusCode = response.StatusCode;
            var result = await response.Content?.ReadAsStringAsync();
            return result;
        }

        private HttpClient httpClient;

        private HttpClient GetClient(bool useToken)
        {
            if (httpClient == null || MultithreadingMode)
            {
                var clientHandler = SslSertificateValidator.BuildHttpClientHandler();
                httpClient = new HttpClient(clientHandler);
                httpClient.Timeout = TimeSpan.FromSeconds(RequestTimeout);
            }

            if (useToken)
            {
                AddToken(httpClient);
            }
            else
            {
                RemoveToken(httpClient);
            }
            return httpClient;
        }

        private string BuildRequest(string apiRequest)
        {
            var fullRequest = string.Format("{0}/{1}", HostAddress.TrimEnd('/'), apiRequest.TrimStart('/'));
            return fullRequest;
        }

        private void AddToken(HttpClient client)
        {
            var values = new List<string>() { "Bearer " + AuthorizationToken };
            client.DefaultRequestHeaders.Add(AuthorizationHeader, values);
        }

        private void RemoveToken(HttpClient client)
        {
            if (client.DefaultRequestHeaders.Contains(AuthorizationHeader))
            {
                client.DefaultRequestHeaders.Remove(AuthorizationHeader);
            }
        }

    }
}
