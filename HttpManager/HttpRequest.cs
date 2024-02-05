using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace HttpManager
{
	public class HttpRequest
	{
		private HttpClient _client;

		public HttpRequest(HttpClient client)
		{
			_client = client;
		}

		public void AddAuthorization(string token)
		{
			_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
		}

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

		public async Task<string> PostAsync(string url, string content)
		{
			try
			{
				var response = await _client.PostAsync(url, new StringContent(content));
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public async Task<string> PutAsync(string url, string content)
		{
			try
			{
				var response = await _client.PutAsync(url, new StringContent(content));
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
	}
}
