using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public interface IHttpManager
	{
		public IDictionary<string, string> Parameters { get; }

		void SetAuthorization(string scheme, string token);
		void SetHeader(string key, string value);
		void SetHeader(string key, IEnumerable<string> value);
		void SetBaseAddress(string url);
		void SetParameter(string key, string value);
		void AddParameters(IDictionary<string,string> parameters);
		bool RemoveParameter(string key);
		Task<string> GetAsync(string url, IDictionary<string, string> parameters = null);
		Task<string> PostAsync(string url, HttpContent content);
		Task<string> PutAsync(string url, HttpContent content);
		Task<string> DeleteAsync(string url);
	}
}
