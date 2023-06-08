using System;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace HttpClient
{
	public class HttpRequest : IDisposable
	{
		public HttpClient Client { get; private set; }

		public void Dispose()
		{
			Client.Dispose();
		}

		public Task GetAsync(string url)
		{

		}

		public Task PostAsync(string url, string body)
		{

		}
	}
}
