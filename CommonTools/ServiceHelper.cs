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
		public static IHttpClientFactory GetHttpClientFactory()
		{
			var services = new ServiceCollection();
			return services
						.AddHttpClient()
						.BuildServiceProvider()
						.GetService<IHttpClientFactory>();
		}
	}
}
