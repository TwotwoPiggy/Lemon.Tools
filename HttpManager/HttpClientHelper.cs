using CommonTools;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace HttpManager
{
	/*
		https://jsonplaceholder.typicode.com：用于测试和原型设计的免费虚设 API。
		https://www.example.com：此域用于文档中的说明性示例。

		GET https://jsonplaceholder.typicode.com/todos/3 HTTP/1.1
		{
			"userId": 1,
			"id": 3,
			"title": "fugiat veniam minus",
			"completed": false
		}

		GET https://jsonplaceholder.typicode.com/todos?userId=1&completed=false HTTP/1.1

	 */
	public class HttpClientHelper : IHttpManager
	{
		private readonly IHttpClientFactory _clientFactory;
		private HttpClient _client;
		/// <summary>
		/// the parameters used to query
		/// </summary>
		private IDictionary<string, string> _parameters;
		public IDictionary<string, string> Parameters => _parameters;


		public HttpClientHelper(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
			Initialize();
		}

		public void Initialize()
		{
			_client ??= _clientFactory.CreateClient();
			_parameters ??= new Dictionary<string, string>();
		}

		#region Authorization
		public void SetAuthorization(string scheme, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
		}
		#endregion

		#region Address
		public void SetBaseAddress(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentNullException(nameof(url), "Please provide a valid url for this request");
			}
			_client.BaseAddress = new Uri(url);
		}
		#endregion

		#region Headers
		public void SetHeader(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Please provide a valid Header Key for this request");
			}
			if (_client.DefaultRequestHeaders.Contains(key))
			{
				_client.DefaultRequestHeaders.Remove(key);
			}
			_client.DefaultRequestHeaders.Add(key, value);
		}

		public void SetHeader(string key, IEnumerable<string> value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Please provide a valid Header Key for this request");
			}
			if (_client.DefaultRequestHeaders.Contains(key))
			{
				_client.DefaultRequestHeaders.Remove(key);
			}
			_client.DefaultRequestHeaders.Add(key, value);
		}

		#endregion

		#region Parameters
		public void SetParameter(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Please provide a valid Parameter Key for this request");
			}
			_parameters[key] = value;
		}

		public void AddParameters(IDictionary<string, object> parameters)
		{
			if (parameters.Any(kv => string.IsNullOrWhiteSpace(kv.Key)))
			{
				throw new ArgumentNullException("Key", "One or more keys are null. Please provide valid keys in this list");
			};
			_parameters.ConcatDictionary(parameters);
		}

		public bool RemoveParameter(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key), "Please provide a valid Parameter Key for this request");
			}
			return _parameters.Remove(key);
		}

		#endregion

		#region public requests

		public async Task<string> GetAsync(string url, IDictionary<string,string> parameters = null)
		{
			try
			{
				var requestUrl = string.IsNullOrWhiteSpace(url) ? _client.BaseAddress.ToString() : url;
				if ((parameters ??= _parameters) != null)
				{
					requestUrl = QueryHelpers.AddQueryString(requestUrl, parameters ?? _parameters);
				}

				var response = await _client.GetAsync(requestUrl);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}


		public async Task<string> PostAsync(string url, HttpContent content)
		{
			try
			{
				var response = await _client.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> PutAsync(string url, HttpContent content)
		{
			try
			{
				var response = await _client.PutAsync(url, content);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> DeleteAsync(string url)
		{
			try
			{
				var response = await _client.DeleteAsync(url);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion
	}
}
