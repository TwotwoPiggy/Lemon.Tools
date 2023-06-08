using AutoCursorTool;
using CommonTools;
using System;
using System.Timers;

namespace LoafOnTheJob.Service
{
	public class MouseMoveService
	{
		private int _handleNum = 0;
		private readonly CursorHelper _cursorHelper;
		private readonly TimerManager _timerManager;
		public MouseMoveService(CursorHelper cursorHelper, TimerManager timerManager)
		{
			_cursorHelper = cursorHelper;
			_timerManager = timerManager;
		}

		public void MoveMouse()
		{
			_cursorHelper.InitCursor(1, 1);

			_timerManager.SetTimer(KeepDeviceActiveEvent, default);
			_timerManager.EnableTimer();
		}

		public bool StopMoveMouse()
		{
			_timerManager.DisableTimer();
			return true;
		}

		private void KeepDeviceActiveEvent(Object source, ElapsedEventArgs e)
		{
			if (_handleNum >= 50)
			{
				_cursorHelper.InitCursor(1, 1);
				_handleNum = 0;
			}
			_cursorHelper.MoveCursor(10, 10);
			_cursorHelper.ClickMouse(User32Input.MouseEvent_LeftUp | User32Input.MouseEvent_LeftDown, 0, 0, 0, 0);
			_handleNum++;
		}
	}
}
