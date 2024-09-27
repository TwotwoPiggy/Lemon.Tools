using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace HttpManager
{
	public abstract class HttpRequest: IHttpManager
	{
		protected HttpClient _client;

		public IEnumerable<KeyValuePair<string, string>> Parameters => throw new NotImplementedException();

		public HttpRequest(HttpClient client)
		{
			_client = client;
		}

		#region Headers

		public void AddAuthorization(string scheme, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
		}

		public void AddHeader(string key, string value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key));
			}
			_client.DefaultRequestHeaders.Add(key, value);
		}
		public void AddParameter(string key, string value)
		{
			throw new NotImplementedException();
		}
		public void AddParameters(IEnumerable<KeyValuePair<string, string>> parameters)
		{
			throw new NotImplementedException();
		}
		public void RemoveParameter(string key)
		{
			throw new NotImplementedException();
		}

		public void UpdateParameter(string key, string value)
		{
			throw new NotImplementedException();
		}


		#endregion


		#region public Http requests

		public async Task<string> GetAsync(string url)
		{
			try
			{
				var response = await _client.GetAsync(url);
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
