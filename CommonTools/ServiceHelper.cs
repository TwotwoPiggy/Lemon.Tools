using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public class ServiceHelper
	{
		private static readonly Lazy<IHttpClientFactory> _httpClientFactoryLazy = new Lazy<IHttpClientFactory>(() =>
		{
			var services = new ServiceCollection();
			return services.AddHttpClient().BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
		});

		public static IHttpClientFactory GetHttpClientFactory()
		{
			return _httpClientFactoryLazy.Value;
		}
	}
}
