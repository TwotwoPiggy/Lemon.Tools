using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public class MultipartHttpRequest : HttpRequest
	{
		//MultipartContent
		public MultipartHttpRequest(HttpClient client) : base(client) { }

		public async Task<string> PostAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			throw new NotImplementedException();
		}

		public async Task<string> PutAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			throw new NotImplementedException();
		}
	}
}
