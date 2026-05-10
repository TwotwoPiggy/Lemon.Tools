using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    /// <summary>
    /// 定时器管理器，提供周期性任务执行功能。
    /// <para>
    /// 基于 <see cref="PeriodicTimer"/> 实现，支持异步操作和取消。
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// 时间常量说明：
    /// <list type="bullet">
    /// <item><description><see cref="SecondsPerMinute"/>: 一分钟的秒数 (60)</description></item>
    /// <item><description><see cref="SecondsPerHour"/>: 一小时的秒数 (3600)</description></item>
    /// <item><description><see cref="SecondsPerDay"/>: 一天的秒数 (86400)</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public sealed class TimerManager : IDisposable, IAsyncDisposable
    {
        #region Constants

        /// <summary>
        /// 一分钟的秒数。
        /// </summary>
        public const int SecondsPerMinute = 60;

        /// <summary>
        /// 一小时的秒数。
        /// </summary>
        public const int SecondsPerHour = 3600;

        /// <summary>
        /// 一天的秒数。
        /// </summary>
        public const int SecondsPerDay = 86400;

        #endregion

        #region Properties

        /// <summary>
        /// 获取定时器间隔。
        /// </summary>
        public TimeSpan Interval { get; }

        /// <summary>
        /// 获取定时器是否正在运行。
        /// </summary>
        public bool IsRunning => _isRunning && !_cts.IsCancellationRequested;

        /// <summary>
        /// 获取定时器是否为周期性（重复执行）。
        /// </summary>
        public bool IsRepeating { get; private set; } = true;

        private PeriodicTimer? _timer;
        private CancellationTokenSource _cts = new();
        private Task? _timerTask;
        private volatile bool _isRunning;
        private volatile bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化 <see cref="TimerManager"/> 类的新实例。
        /// </summary>
        /// <param name="intervalSeconds">间隔秒数。</param>
        public TimerManager(int intervalSeconds)
            : this(TimeSpan.FromSeconds(intervalSeconds))
        {
        }

        /// <summary>
        /// 初始化 <see cref="TimerManager"/> 类的新实例。
        /// </summary>
        /// <param name="interval">间隔时间。</param>
        public TimerManager(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Interval must be greater than zero.");
            }

            Interval = interval;
            _timer = new PeriodicTimer(interval);
        }

        /// <summary>
        /// 初始化 <see cref="TimerManager"/> 类的新实例（默认间隔 1 秒）。
        /// </summary>
        public TimerManager()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 启动定时器，执行指定的回调。
        /// </summary>
        /// <param name="callback">每次触发时执行的回调。</param>
        /// <param name="isRepeating">是否重复执行（默认为 true）。</param>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        /// <exception cref="InvalidOperationException">定时器已在运行。</exception>
        public void Start(Action callback, bool isRepeating = true)
        {
            ArgumentNullException.ThrowIfNull(callback);

            StartCore(async () =>
            {
                callback();
                await Task.CompletedTask.ConfigureAwait(false);
            }, isRepeating);
        }

        /// <summary>
        /// 启动定时器，执行指定的异步回调。
        /// </summary>
        /// <param name="callback">每次触发时执行的异步回调。</param>
        /// <param name="isRepeating">是否重复执行（默认为 true）。</param>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        /// <exception cref="InvalidOperationException">定时器已在运行。</exception>
        public void Start(Func<Task> callback, bool isRepeating = true)
        {
            ArgumentNullException.ThrowIfNull(callback);
            StartCore(callback, isRepeating);
        }

        /// <summary>
        /// 启动定时器，执行指定的异步回调（带取消令牌）。
        /// </summary>
        /// <param name="callback">每次触发时执行的异步回调。</param>
        /// <param name="isRepeating">是否重复执行（默认为 true）。</param>
        /// <exception cref="ObjectDisposedException">对象已被释放。</exception>
        /// <exception cref="InvalidOperationException">定时器已在运行。</exception>
        public void Start(Func<CancellationToken, Task> callback, bool isRepeating = true)
        {
            ArgumentNullException.ThrowIfNull(callback);

            StartCore(async () =>
            {
                await callback(_cts.Token).ConfigureAwait(false);
            }, isRepeating);
        }

        private void StartCore(Func<Task> callback, bool isRepeating)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(TimerManager));
            if (_isRunning) throw new InvalidOperationException("Timer is already running.");

            IsRepeating = isRepeating;
            _isRunning = true;

            _timerTask = ExecutePeriodicAsync(callback);
        }

        private async Task ExecutePeriodicAsync(Func<Task> callback)
        {
            try
            {
                do
                {
                    if (_cts.Token.IsCancellationRequested)
                        break;

                    await callback().ConfigureAwait(false);

                    if (!IsRepeating)
                        break;

                } while (await _timer!.WaitForNextTickAsync(_cts.Token).ConfigureAwait(false));
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _isRunning = false;
            }
        }

        /// <summary>
        /// 停止定时器。
        /// </summary>
        public void Stop()
        {
            if (!_isRunning) return;

            _cts.Cancel();
            _timerTask?.GetAwaiter().GetResult();
            ResetCancellationToken();
        }

        /// <summary>
        /// 异步停止定时器。
        /// </summary>
        /// <param name="cancellationToken">取消令牌。</param>
        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (!_isRunning) return;

            _cts.Cancel();

            if (_timerTask != null)
            {
                try
                {
                    await _timerTask.WaitAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                }
            }

            ResetCancellationToken();
        }

        private void ResetCancellationToken()
        {
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            _timer?.Dispose();
            _timer = new PeriodicTimer(Interval);
            _timerTask = null;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            Stop();
            _timer?.Dispose();
            _cts.Dispose();

            _disposed = true;
        }

        /// <summary>
        /// 异步释放资源。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            await StopAsync().ConfigureAwait(false);
            _timer?.Dispose();
            _cts.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
