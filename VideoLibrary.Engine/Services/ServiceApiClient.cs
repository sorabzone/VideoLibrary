using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;

namespace VideoLibrary.Engine.Services
{
    public interface IServiceApiClient
    {
        HttpClient GetHttpClient(string apiName);
    }

    public class ServiceApiClient : IServiceApiClient
    {
        private readonly ConcurrentDictionary<string, HttpClient> _httpClients = new ConcurrentDictionary<string, HttpClient>();
        private readonly Dictionary<string, string> _externalAPIs = new Dictionary<string, string>();

        public ServiceApiClient()
        {
            _externalAPIs.Add("WEBJET", "http://webjetapitest.azurewebsites.net");
        }

        public HttpClient GetHttpClient(string apiName)
        {
            var client = _httpClients.GetOrAdd(apiName, delegate
            {
                if (_httpClients.ContainsKey(apiName))
                    return _httpClients[apiName];

                var serviceEndpoint = new Uri(_externalAPIs[apiName]);
                var httpClient = new HttpClient { BaseAddress = serviceEndpoint };
                httpClient.Timeout = TimeSpan.FromSeconds(3);
                return httpClient;
            });

            return client;
        }
    }
}
