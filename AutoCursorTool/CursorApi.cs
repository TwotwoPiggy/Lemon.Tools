using System;
using System.Runtime.InteropServices;
using static AutoCursorTool.User32Input;

namespace AutoCursorTool
{
	public class CursorApi
	{
		const string DllName = "user32.dll";
		[DllImport(DllName, SetLastError = true)]
		public static extern bool SetCursorPos(int x, int y);

		/// <summary>
		/// 将创建指定窗口的线程引入前台并激活窗口
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		[DllImport(DllName)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport(DllName)]
		public static extern IntPtr FindWindowA(string className, string windowName);


		[DllImport(DllName)]
		public static extern uint SendInput(uint inputsNumber, Input[] inputs, int sizeOfInputStructure);

		[DllImport(DllName, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


	}
}
