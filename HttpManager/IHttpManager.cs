using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public interface IHttpManager
	{
		Task<string> GetAsync(string url);
		Task<string> PostAsync(string url, string content);
		Task<string> PutAsync(string url, string content);
		Task<string> DeleteAsync(string url);
	}
}
