using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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

		public static void AddAttribute(string path, FileAttributes attribute)
		{
			//// 修改重要文件前检查
			//if (HasAttribute("system.dll", FileAttributes.System))
			//{
			//	Console.WriteLine("警告：正在修改系统文件！");
			//}
			var current = File.GetAttributes(path);
			File.SetAttributes(path, current | attribute);
		}

		public static void RemoveAttribute(string path, FileAttributes attribute)
		{
			//// 修改重要文件前检查
			//if (HasAttribute("system.dll", FileAttributes.System))
			//{
			//	Console.WriteLine("警告：正在修改系统文件！");
			//}
			var current = File.GetAttributes(path);
			File.SetAttributes(path, current & ~attribute);
		}

		public static bool HasAttribute(string path, FileAttributes attribute)
		{
			var current = File.GetAttributes(path);
			return (current & attribute) == attribute;
		}

		public static string GetAttributesString(string path)
		{
			var attrs = File.GetAttributes(path);
			return attrs.ToString().Replace(", ", "|");
		}
	}
}
