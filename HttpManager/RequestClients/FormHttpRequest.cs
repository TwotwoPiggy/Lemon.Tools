using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public class FormHttpRequest : HttpRequest
	{

		public FormHttpRequest(HttpClient client): base(client){}

		public async Task<string> PostAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			try
			{
				var response = await base.PostAsync(url, new FormUrlEncodedContent(parameters));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<string> PutAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			try
			{
				var response = await base.PutAsync(url, new FormUrlEncodedContent(parameters));
				return response;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
