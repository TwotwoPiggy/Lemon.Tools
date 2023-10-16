using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonTools
{
	public static  class SystemManager
	{
		public static void ShutDownMachine(int seconds)
		{
			var arg = $"-s -t {seconds}";
			try
			{
				Process.Start("ShutDown", arg);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static void Cancel()
		{
			try
			{
				Process.Start("shutdown", "-a");
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
