using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    /// <summary>
    /// 重试策略选项配置。
    /// </summary>
    public class RetryOptions
    {
        /// <summary>
        /// 最大重试次数。默认为 3。
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// 初始延迟时间。默认为 1 秒。
        /// </summary>
        public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 退避乘数（用于指数退避）。默认为 2.0。
        /// </summary>
        public double BackoffMultiplier { get; set; } = 2.0;

        /// <summary>
        /// 最大延迟时间。默认为 30 秒。
        /// </summary>
        public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 要处理的异常类型。默认包含 HttpRequestException。
        /// </summary>
        public List<Type> HandleExceptions { get; set; } = new() { typeof(HttpRequestException) };

        /// <summary>
        /// 要处理的 HTTP 状态码。默认包含 BadGateway。
        /// </summary>
        public List<HttpStatusCode> HandleStatusCodes { get; set; } = new()
        {
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        /// <summary>
        /// 重试时的回调函数。
        /// </summary>
        public Action<Exception, TimeSpan, int, Context>? OnRetry { get; set; }
    }

    /// <summary>
    /// 弹性策略管理器，提供可配置的重试策略。
    /// <para>
    /// 基于 Polly 库实现，支持同步和异步操作。
    /// </para>
    /// </summary>
    public static class RetryManager
    {
        private static readonly RetryOptions _defaultOptions = new();

        /// <summary>
        /// 使用默认选项创建异步重试策略。
        /// </summary>
        /// <returns>异步重试策略。</returns>
        public static AsyncRetryPolicy CreateDefaultAsyncPolicy()
        {
            return CreateAsyncPolicy(_defaultOptions);
        }

        /// <summary>
        /// 使用指定选项创建异步重试策略。
        /// </summary>
        /// <param name="options">重试选项。</param>
        /// <returns>异步重试策略。</returns>
        public static AsyncRetryPolicy CreateAsyncPolicy(RetryOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            var delays = CalculateDelays(options);

            return Policy
                .Handle<Exception>(ex => options.HandleExceptions.Any(t => t.IsAssignableFrom(ex.GetType())))
                .WaitAndRetryAsync(
                    retryCount: options.MaxRetries,
                    sleepDurationProvider: (retryAttempt, context) => delays[Math.Min(retryAttempt - 1, delays.Length - 1)],
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        options.OnRetry?.Invoke(exception, timeSpan, retryCount, context);
                    });
        }

        /// <summary>
        /// 使用指定选项创建针对 HttpResponseMessage 的异步重试策略。
        /// </summary>
        /// <param name="options">重试选项。</param>
        /// <returns>异步重试策略。</returns>
        public static AsyncRetryPolicy<HttpResponseMessage> CreateHttpAsyncPolicy(RetryOptions? options = null)
        {
            options ??= _defaultOptions;
            var delays = CalculateDelays(options);

            var builder = Policy<HttpResponseMessage>
                .Handle<Exception>(ex => options.HandleExceptions.Any(t => t.IsAssignableFrom(ex.GetType())));

            foreach (var statusCode in options.HandleStatusCodes)
            {
                builder = builder.OrResult(r => r.StatusCode == statusCode);
            }

            return builder.WaitAndRetryAsync(
                retryCount: options.MaxRetries,
                sleepDurationProvider: (retryAttempt, context) => delays[Math.Min(retryAttempt - 1, delays.Length - 1)],
                onRetry: (outcome, timeSpan, retryCount, context) =>
                {
                    options.OnRetry?.Invoke(outcome.Exception, timeSpan, retryCount, context);
                });
        }

        /// <summary>
        /// 使用默认选项执行异步操作。
        /// </summary>
        /// <param name="action">要执行的操作。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        public static async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            var policy = CreateDefaultAsyncPolicy();
            await policy.ExecuteAsync(async ct => await action().ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 使用默认选项执行异步操作并返回结果。
        /// </summary>
        /// <typeparam name="T">返回类型。</typeparam>
        /// <param name="action">要执行的操作。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>操作结果。</returns>
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            var policy = CreateDefaultAsyncPolicy();
            return await policy.ExecuteAsync(async ct => await action().ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 使用指定选项执行异步操作。
        /// </summary>
        /// <param name="options">重试选项。</param>
        /// <param name="action">要执行的操作。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        public static async Task ExecuteAsync(RetryOptions options, Func<Task> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(action);

            var policy = CreateAsyncPolicy(options);
            await policy.ExecuteAsync(async ct => await action().ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 使用指定选项执行异步操作并返回结果。
        /// </summary>
        /// <typeparam name="T">返回类型。</typeparam>
        /// <param name="options">重试选项。</param>
        /// <param name="action">要执行的操作。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>操作结果。</returns>
        public static async Task<T> ExecuteAsync<T>(RetryOptions options, Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(action);

            var policy = CreateAsyncPolicy(options);
            return await policy.ExecuteAsync(async ct => await action().ConfigureAwait(false), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行 HTTP 请求并自动重试。
        /// </summary>
        /// <param name="httpClient">HTTP 客户端。</param>
        /// <param name="request">HTTP 请求。</param>
        /// <param name="options">重试选项（可选）。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>HTTP 响应。</returns>
        public static async Task<HttpResponseMessage> ExecuteHttpRequestAsync(
            HttpClient httpClient,
            HttpRequestMessage request,
            RetryOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(request);

            var policy = CreateHttpAsyncPolicy(options);

            return await policy.ExecuteAsync(async ct =>
            {
                var clonedRequest = await CloneRequestAsync(request).ConfigureAwait(false);
                return await httpClient.SendAsync(clonedRequest, ct).ConfigureAwait(false);
            }, cancellationToken).ConfigureAwait(false);
        }

        private static TimeSpan[] CalculateDelays(RetryOptions options)
        {
            var delays = new TimeSpan[options.MaxRetries];
            for (int i = 0; i < options.MaxRetries; i++)
            {
                var delay = TimeSpan.FromSeconds(
                    Math.Min(
                        options.InitialDelay.TotalSeconds * Math.Pow(options.BackoffMultiplier, i),
                        options.MaxDelay.TotalSeconds));
                delays[i] = delay;
            }
            return delays;
        }

        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
                VersionPolicy = request.VersionPolicy
            };

            if (request.Content != null)
            {
                var content = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                clone.Content = new ByteArrayContent(content);

                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var option in request.Options)
            {
                clone.Options.Set(new HttpRequestOptionsKey<object?>(option.Key), option.Value);
            }

            return clone;
        }
    }
}
