using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpManager
{
	public class TokenManager : RestClient
	{
		public TokenManager(IHttpClientFactory clientFactory) : base(clientFactory)
		{
		}

		public async Task<Token> GetTokenAsync()
		{
			var url = _restConfig.AuthUrl ?? "";
			var authHeader = new AuthenticationHeaderValue("Basic", _restConfig.ServiceName);//todo

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "username",_restConfig.Username },
				{ "password",_restConfig.Password },
				{ "grant_type","password" }
			});


			try
			{
				var token = await PostAsync<Token>(url, content, authHeader);
				if (token == null)
				{
					//todo: retry
				}
				return token;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<Token> RefreshTokenAsync(string refreshToken)
		{
			var url = _restConfig.AuthUrl ?? "";
			var authHeader = new AuthenticationHeaderValue("Basic", _restConfig.ServiceName);//todo

			var content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "refresh_token", refreshToken },
				{ "grant_type","refresh_token" }
			});


			try
			{
				var token = await PostAsync<Token>(url, content, authHeader);
				if (token == null)
				{
					//todo: retry
				}
				return token;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
