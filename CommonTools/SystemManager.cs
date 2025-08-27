using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonTools
{
	public static class SystemManager
	{
		#region public methods
		public static void ShutDownMachine(int hours = 0, int minutes = 0, int seconds = 0, bool forceActionWithoutWarning = false)
		{
			var totalSeconds = hours * 3600 + minutes * 60 + seconds;
			var arg = $"-s {(forceActionWithoutWarning ? "-f" : string.Empty)} -t {totalSeconds}";
			ExecuteCommand("ShutDown", arg);
		}

		public static void RestartMachine(int hours = 0, int minutes = 0, int seconds = 0, bool forceActionWithoutWarning = false)
		{
			var totalSeconds = hours * 3600 + minutes * 60 + seconds;
			var arg = $"-r {(forceActionWithoutWarning ? "-f" : string.Empty)} -t {totalSeconds}";
			ExecuteCommand("ShutDown", arg);
		}

		public static void Cancel()
		{
			ExecuteCommand("ShutDown", "-a");
		}

		/// <summary>
		/// Modifies the value of a service's entries in the registry and in the Service Control Manager database.
		/// <see cref="https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/sc-config"/>
		/// </summary>
		public static void SetServiceValue(string serviceName, string arguments)
		{
			var arg = $"config {serviceName} {arguments}";
			ExecuteCommand("sc", arg);
		}

		public static string GetServiceValue(string serviceName)
		{
			var arg = $"query {serviceName}";
			return ExecuteCommandWithResult("sc", arg);
		}
		#endregion

		#region private methods
		private static void ExecuteCommand(string command, string arg = null)
		{
			try
			{
				Process.Start(command, arg);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static string ExecuteCommandWithResult(string command, string arg = null)
		{
			try
			{
				var startInfo = new ProcessStartInfo(command)
				{
					Verb = "runas",
					Arguments = arg,
					FileName = command,
					UseShellExecute = false,
					RedirectStandardOutput = true
				};
				using var process = new Process();
				process.StartInfo = startInfo;
				process.Start();
				return process.StandardOutput.ReadToEnd();
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion
	}
}
