using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonTools
{
	public static class FileManager
	{
		#region extension methods
		public static bool IsFile(this string path)
		{
			return File.Exists(path);
		}

		public static bool IsDirectory(this string path)
		{
			return Directory.Exists(path);
		}

		public static string GetExtension(this string fileName)
		{
			return Path.GetExtension(fileName);
		}
		#endregion

		public static IEnumerable<string> GetFiles(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				throw new DirectoryNotFoundException($"{directoryPath} is not found, Please check if it exists");
			}
			return Directory.GetFiles(directoryPath).AsEnumerable();
		}

		public static IEnumerable<string> GetDirectories(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				throw new DirectoryNotFoundException($"{directoryPath} is not found, Please check if it exists");
			}
			return Directory.GetDirectories(directoryPath).AsEnumerable();
		}

		public static void RenameFile(string oldFileName, string newFileName)
		{
			try
			{
				File.Move(oldFileName, newFileName, true);
			}
			catch (Exception)
			{
				throw;
			}

		}
	}
}
