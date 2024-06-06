using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public class ByteArrayHttpRequest:HttpRequest
	{
		public ByteArrayHttpRequest(HttpClient client) : base(client) { }

		public async Task<string> PostAsync(string url, byte[] content)
		{
			try
			{
				var response = await base.PostAsync(url, new ByteArrayContent(content));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> PostAsync(string url, byte[] content, int offset, int count)
		{
			try
			{
				var response = await base.PostAsync(url, new ByteArrayContent(content, offset, count));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> PutAsync(string url, byte[] content)
		{
			try
			{
				var response = await base.PutAsync(url, new ByteArrayContent(content));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> PutAsync(string url, byte[] content, int offset, int count)
		{
			try
			{
				var response = await base.PostAsync(url, new ByteArrayContent(content, offset, count));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
