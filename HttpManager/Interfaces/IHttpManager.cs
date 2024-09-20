using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager.Interfaces
{
    public interface IHttpManager
    {
        IEnumerable<KeyValuePair<string, string>> Parameters { get; }
        void AddParameter(string key, string value);
        void RemoveParameter(string key);
        void UpdateParameter(string key, string value);
        void AddParameters(IEnumerable<KeyValuePair<string, string>> parameters);
        void AddHeader(string key, string value);
        void AddAuthorization(string scheme, string token);
        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, HttpContent content);
        Task<string> PutAsync(string url, HttpContent content);
        Task<string> DeleteAsync(string url);
    }
}
