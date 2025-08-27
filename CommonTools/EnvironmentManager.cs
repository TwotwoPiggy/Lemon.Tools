using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
	public static class EnvironmentManager
	{
		#region properties
		/// <summary>
		/// the path of the application's exe file.
		/// </summary>
		public static string ExePath => Environment.ProcessPath ?? string.Empty;

		public static string StartupPath => AppDomain.CurrentDomain.BaseDirectory;

		#endregion

		public static string GetRelativePath(string originPath)
		{
			return Path.GetRelativePath(StartupPath, originPath);
		}
	}
}
