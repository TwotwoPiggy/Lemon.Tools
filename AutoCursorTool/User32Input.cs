using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoCursorTool
{
	public class User32Input
	{
		public const int MouseEvent_Move = 0x0001;
		public const int MouseEvent_LeftDown = 0x0002;
		public const int MouseEvent_LeftUp = 0x0004;
		public const int MouseEvent_RightDown = 0x0008;
		public const int MouseEvent_RightUp = 0x0010;
		public const int MouseEvent_MiddleDown = 0x0020;
		public const int MouseEvent_MiddleUp = 0x0040;
		public const int MouseEvent_XDown = 0x0080;
		public const int MouseEvent_XDp = 0x0100;
		public const int MouseEvent_Wheel = 0x0800;
		public const int MouseEvent_HWheel = 0x1000;
		public const int MouseEvent_Move_Nocoalesce = 0x2000;
		public const int MouseEvent_VirtualDesk = 0x4000;
		public const int MouseEvent_Absolute = 0x8000;

		[StructLayout(LayoutKind.Sequential)]//https://www.cnblogs.com/lonelyDog/archive/2012/02/02/2335432.html
		public struct Input
		{
			public SendInputEventType Type;
			public MouseAndKeyBoardInput MouseKeyboardInput;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct MouseAndKeyBoardInput
		{
			[FieldOffset(0)]
			public MouseInput MouseInput;

			[FieldOffset(0)]
			public KeyboardInput KeyboardInput;

			[FieldOffset(0)]
			public HardwareInput HardWareInput;
		}

		[Flags]
		public enum SendInputEventType : uint
		{
			InputMouse = 0,
			InputKeyboard = 1,
			InputHardware = 2
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MouseInput
		{
			public int dx;
			public int dy;
			public uint mouseData;
			public ushort dwFlags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct KeyboardInput
		{
			public ushort wVk;
			public ushort wScan;
			public uint dwFlags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HardwareInput
		{
			public int uMsg;
			public short wParamL;
			public short wParamH;
		}


		public enum EventCode : ushort
		{
			MOUSEEVENTF_MOVE = 0x0001,
			MOUSEEVENTF_LEFTDOWN = 0x0002,
			MOUSEEVENTF_LEFTUP = 0x0004,
			MOUSEEVENTF_RIGHTDOWN = 0x0008,
			MOUSEEVENTF_RIGHTUP = 0x0010,
			MOUSEEVENTF_MIDDLEDOWN = 0x0020,
			MOUSEEVENTF_MIDDLEUP = 0x0040,
			MOUSEEVENTF_XDOWN = 0x0080,
			MOUSEEVENTF_XUP = 0x0100,
			MOUSEEVENTF_WHEEL = 0x0800,
			MOUSEEVENTF_HWHEEL = 0x1000,
			MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
			MOUSEEVENTF_VIRTUALDESK = 0x4000,
			MOUSEEVENTF_ABSOLUTE = 0x8000
		}
	}
}
