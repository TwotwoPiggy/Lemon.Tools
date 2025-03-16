using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace CommonTools
{
	public static class FileManager
	{
        #region extension methods
        public static bool IsOrExistFile(this string path)
		{
			return File.Exists(path);
		}

		public static bool IsOrExistDirectory(this string path)
		{
			return Directory.Exists(path);
		}

		public static string GetExtension(this string fileName)
		{
			return Path.GetExtension(fileName);
		}

		public static string GetPathWithOutExtension(this string fileName)
		{
			return Path.GetFileNameWithoutExtension(fileName);
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

		public static void MoveFile(string sourceFilePath, string destinationFilePath)
		{
			try
			{
				if (!File.Exists(sourceFilePath))
				{
					throw new FileNotFoundException($"{sourceFilePath} is not found, Please check if it exists");
				}
				// 移动文件
				File.Move(sourceFilePath, destinationFilePath);

			}
			catch (Exception)
			{
				throw;
			}
		}

		//public static string GetPath(string path, string fileName, string type)
		//{
		//	path = string.IsNullOrWhiteSpace(path) ? EnvironmentManager
		//}
	}
}
