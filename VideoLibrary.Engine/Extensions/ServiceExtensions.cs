using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoLibrary.Engine.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task<O> SendSimpleAsync<O>(this HttpClient client, string route, Dictionary<string, string> customHeaders = null)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, route))
            {
                if (customHeaders != null)
                {
                    foreach (var h in customHeaders)
                    {
                        request.Headers.Add(h.Key, h.Value);
                    }
                }

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode) return default(O);

                    return await response.Content.ReadAsAsync<O>();
                }
            }
        }
    }
}
