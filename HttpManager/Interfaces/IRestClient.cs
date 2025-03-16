using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager.Interfaces
{
    public interface IRestClient
    {
        Task<T> GetAsync<T>(string requestUrl, AuthenticationHeaderValue token, Dictionary<string, string> headers = null);
        Task<T> PostAsync<T>(string url, HttpContent content = null, AuthenticationHeaderValue token = null, Dictionary<string, string> headers = null);
		Task<T> SendRequestAsync<T>(HttpRequestMessage request, AuthenticationHeaderValue token, Dictionary<string, string> headers);
    }
}
