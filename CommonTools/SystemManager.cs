using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    /// <summary>
    /// 系统管理器，提供系统关机、重启和服务管理功能。
    /// <para>
    /// 仅支持 Windows 平台。
    /// </para>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class SystemManager
    {
        #region Power Management

        /// <summary>
        /// 关闭计算机。
        /// </summary>
        /// <param name="hours">延迟小时数。</param>
        /// <param name="minutes">延迟分钟数。</param>
        /// <param name="seconds">延迟秒数。</param>
        /// <param name="forceActionWithoutWarning">是否强制执行而不警告用户。</param>
        /// <exception cref="InvalidOperationException">无法执行关机命令。</exception>
        public static void ShutDownMachine(int hours = 0, int minutes = 0, int seconds = 0, bool forceActionWithoutWarning = false)
        {
            var totalSeconds = hours * 3600 + minutes * 60 + seconds;
            var arguments = $"-s {(forceActionWithoutWarning ? "-f" : string.Empty)} -t {totalSeconds}".Trim();
            ExecuteCommand("shutdown", arguments);
        }

        /// <summary>
        /// 重启计算机。
        /// </summary>
        /// <param name="hours">延迟小时数。</param>
        /// <param name="minutes">延迟分钟数。</param>
        /// <param name="seconds">延迟秒数。</param>
        /// <param name="forceActionWithoutWarning">是否强制执行而不警告用户。</param>
        /// <exception cref="InvalidOperationException">无法执行重启命令。</exception>
        public static void RestartMachine(int hours = 0, int minutes = 0, int seconds = 0, bool forceActionWithoutWarning = false)
        {
            var totalSeconds = hours * 3600 + minutes * 60 + seconds;
            var arguments = $"-r {(forceActionWithoutWarning ? "-f" : string.Empty)} -t {totalSeconds}".Trim();
            ExecuteCommand("shutdown", arguments);
        }

        /// <summary>
        /// 取消计划的关机或重启。
        /// </summary>
        /// <exception cref="InvalidOperationException">无法取消关机计划。</exception>
        public static void CancelShutdown()
        {
            ExecuteCommand("shutdown", "-a");
        }

        /// <summary>
        /// 注销当前用户。
        /// </summary>
        /// <param name="forceActionWithoutWarning">是否强制执行。</param>
        /// <exception cref="InvalidOperationException">无法执行注销命令。</exception>
        public static void LogOff(bool forceActionWithoutWarning = false)
        {
            var arguments = forceActionWithoutWarning ? "-l -f" : "-l";
            ExecuteCommand("shutdown", arguments);
        }

        #endregion

        #region Service Management

        /// <summary>
        /// 配置 Windows 服务。
        /// </summary>
        /// <param name="serviceName">服务名称。</param>
        /// <param name="arguments">配置参数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceName"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">无法配置服务。</exception>
        /// <remarks>
        /// 参数示例：
        /// <list type="bullet">
        /// <item><description>start= auto - 设置服务为自动启动</description></item>
        /// <item><description>start= demand - 设置服务为手动启动</description></item>
        /// <item><description>start= disabled - 禁用服务</description></item>
        /// </list>
        /// 更多信息请参阅：<see href="https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/sc-config"/>
        /// </remarks>
        public static void SetServiceValue(string serviceName, string arguments)
        {
            ArgumentNullException.ThrowIfNull(serviceName);

            var arg = $"config \"{serviceName}\" {arguments}";
            ExecuteCommand("sc", arg);
        }

        /// <summary>
        /// 获取 Windows 服务状态。
        /// </summary>
        /// <param name="serviceName">服务名称。</param>
        /// <returns>服务状态信息。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceName"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">无法查询服务状态。</exception>
        public static string GetServiceValue(string serviceName)
        {
            ArgumentNullException.ThrowIfNull(serviceName);

            var arguments = $"query \"{serviceName}\"";
            return ExecuteCommandWithResult("sc", arguments);
        }

        /// <summary>
        /// 启动 Windows 服务。
        /// </summary>
        /// <param name="serviceName">服务名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceName"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">无法启动服务。</exception>
        public static void StartService(string serviceName)
        {
            ArgumentNullException.ThrowIfNull(serviceName);

            ExecuteCommand("sc", $"start \"{serviceName}\"");
        }

        /// <summary>
        /// 停止 Windows 服务。
        /// </summary>
        /// <param name="serviceName">服务名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceName"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">无法停止服务。</exception>
        public static void StopService(string serviceName)
        {
            ArgumentNullException.ThrowIfNull(serviceName);

            ExecuteCommand("sc", $"stop \"{serviceName}\"");
        }

        /// <summary>
        /// 异步获取 Windows 服务状态。
        /// </summary>
        /// <param name="serviceName">服务名称。</param>
        /// <param name="cancellationToken">取消令牌。</param>
        /// <returns>服务状态信息。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceName"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">无法查询服务状态。</exception>
        public static async Task<string> GetServiceValueAsync(string serviceName, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(serviceName);

            var arguments = $"query \"{serviceName}\"";
            return await ExecuteCommandWithResultAsync("sc", arguments, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region Process Execution

        private static void ExecuteCommand(string command, string? arguments = null)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments ?? string.Empty,
                UseShellExecute = true,
                CreateNoWindow = true
            };

            if (!process.Start())
            {
                throw new InvalidOperationException($"Failed to start process: {command} {arguments}");
            }
        }

        private static string ExecuteCommandWithResult(string command, string arguments)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"Command failed with exit code {process.ExitCode}: {error}");
            }

            return output;
        }

        private static async Task<string> ExecuteCommandWithResultAsync(string command, string arguments, CancellationToken cancellationToken)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

            var output = await outputTask.ConfigureAwait(false);
            var error = await errorTask.ConfigureAwait(false);

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Command failed with exit code {process.ExitCode}: {error}");
            }

            return output;
        }

        #endregion
    }
}
