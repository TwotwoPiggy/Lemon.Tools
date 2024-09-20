using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class RetryManager
	{
		public static void Retry()
		{
			Policy
				// 1. 指定要处理什么异常
				.Handle<HttpRequestException>()
				//    或者指定需要处理什么样的错误返回
				.OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadGateway)
				// 2. 指定重试次数和重试策略
				.Retry(3, (exception, retryCount, context) =>
				{
					Console.WriteLine($"开始第 {retryCount} 次重试：");
				})
				// 3. 执行具体任务
				.Execute(ExecuteMockRequest);
		}

		private static HttpResponseMessage ExecuteMockRequest()
		{
			throw new NotImplementedException();
		}
	}
}
