using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCursorTool
{
	public class MouseActions
	{
		public enum MouseClickAction
		{
			Left = 0,
			Right = 1,
			DoubleLeft = 2,
			BothRL = 3
		}

		public enum MouseMovingDirection
		{
			Left = 0,
			Right = 1,
			Up = 2,
			Down = 3
		}

		public MouseActions(MouseMovingDirection mouseHorizontalMoving, MouseMovingDirection mouseVerticalMoving)
		{
			_mouseHorizontalMoving = mouseHorizontalMoving;
			_mouseVerticalMoving = mouseVerticalMoving;
		}

		private readonly MouseMovingDirection _mouseHorizontalMoving;
		private readonly MouseMovingDirection _mouseVerticalMoving;

		public int HorizonSgn => _mouseHorizontalMoving == MouseMovingDirection.Left ? -1 : 1;
		public int VerticalSgn => _mouseVerticalMoving == MouseMovingDirection.Up ? -1 : 1;

	}
}
