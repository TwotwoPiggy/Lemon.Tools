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
			ShutDown(totalSeconds);
		}

		public static void Cancel()
		{
			ShutDown("-a");
		}
		#endregion


		#region private methods
		private static void ShutDown(int seconds)
		{
			var arg = $"-s -t {seconds}";
			try
			{
				ShutDown(arg);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static void ShutDown(string arg)
		{
			try
			{
				Process.Start("ShutDown",arg);
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion
	}
}
