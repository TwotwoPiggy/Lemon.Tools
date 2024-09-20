using HttpManager.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public class RestClient : IRestClient
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public RestClient(IHttpClientFactory clientFactory)
		{
			_httpClientFactory = clientFactory;
		}

		public async Task<T> GetAsync<T>(string requestUrl, AuthenticationHeaderValue token, Dictionary<string, string> headers = null)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
			return await SendRequestAsync<T>(request, token, headers);
		}

		public async Task<T> PostAsync<T>(string url, HttpContent content = null, AuthenticationHeaderValue token = null, Dictionary<string, string> headers = null)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content ??= content;
			return await SendRequestAsync<T>(request, token, headers);
		}

		public async Task<T> SendRequestAsync<T>(HttpRequestMessage request, AuthenticationHeaderValue token, Dictionary<string, string> headers)
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				client.DefaultRequestHeaders.Authorization ??= token;
				if (headers != null && headers.Count != 0)
				{
					headers.AsParallel().ForAll(h => client.DefaultRequestHeaders.Add(h.Key, h.Value));
				}

				// without ConfigureAwait() the thread will be blocked
				var response = await client.SendAsync(request).ConfigureAwait(false);
				response.EnsureSuccessStatusCode();

				var responseStr = await response.Content.ReadAsStringAsync();
				return string.IsNullOrWhiteSpace(responseStr) ? default : JsonConvert.DeserializeObject<T>(responseStr);
			}
			catch (HttpRequestException)
			{
				throw;
			}
			catch (HttpIOException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
