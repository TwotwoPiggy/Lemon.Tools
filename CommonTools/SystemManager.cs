using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonTools
{
	public static class SystemManager
	{
		#region public methods
		public static void ShutDownMachine(int hours = 0, int minutes = 0, int seconds = 0)
		{
			var totalSeconds = hours * 3600 + minutes * 60 + seconds;
			var arg = $"-s -t {totalSeconds}";
			ExecuteCommand("ShutDown", arg);
		}

		public static void Cancel()
		{
			ExecuteCommand("ShutDown", "-a");
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

		#endregion
	}
}
