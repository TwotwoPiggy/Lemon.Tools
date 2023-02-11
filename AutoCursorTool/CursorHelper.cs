using System.Drawing;
using System.Windows.Forms;
using static AutoCursorTool.MouseAction;
using static AutoCursorTool.User32Input;

namespace AutoCursorTool
{
	public class CursorHelper
	{

		public bool InitCursor(int x, int y)
		{
			return CursorApi.SetCursorPos(x, y);
		}

		public unsafe uint SendInput(Input[] inputs)
		{
			return CursorApi.SendInput((uint)inputs.Length, inputs, 40);
		}

		public void ClickMouse(uint actions,uint dx,uint dy, uint cButtons, uint dwExtraInfo)
		{
			CursorApi.mouse_event(actions, dx, dy, cButtons, dwExtraInfo);
		}
		public void MoveCursor(int xIncrement, int yIncrement)
		{
			Cursor.Position = new Point(Cursor.Position.X + xIncrement, Cursor.Position.Y + yIncrement);
		}

	}
}
