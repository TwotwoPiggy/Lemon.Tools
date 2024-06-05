using GptApi.Models;
using HttpManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GptApi
{
	public class Gpt3Api
	{
		private string _endPoint;
		private string _apiKey;
		private readonly HttpRequest _httpRequest;
		public Gpt3Api() { }
		public Gpt3Api(HttpRequest httpRequest, string apiKey)
		{
			_httpRequest = httpRequest;
			_apiKey = apiKey;
		}

		public void SetEndPoint(string endPoint)
		{
			_endPoint = endPoint;
		}

		public void SetApiKey(string apiKey)
		{
			_apiKey = apiKey;
		}

		public async Task<Response> GenerateResponseAsync(string endPoint, Request request)
		{
			var settings = new JsonSerializerSettings()
			{
				ContractResolver = new DefaultContractResolver()
			};
			var content = JsonConvert.SerializeObject(request, settings);
			//var result = await _httpRequest.PostAsync(endPoint, content);
			var response = JsonConvert.DeserializeObject<Response>(content);
			return response;
		}

	}
}
