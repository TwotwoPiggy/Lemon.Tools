using System;
using System.Timers;

namespace CommonTools
{
	public class TimerManager
	{
		public readonly int OneHourToSeconds = 60;
		public readonly int OneDayToSeconds = 86400;
		public readonly int OneHour = 60;

		public Timer Timer { get; private set; }

		public TimerManager(int? intervalSeconds = null)
		{
			if (intervalSeconds.HasValue)
			{
				Timer = new Timer(intervalSeconds.Value * 1000);
			}
			else
			{
				Timer = new Timer();
			}
		}

		public void SetTimer(ElapsedEventHandler elapsedEvent, int? intervalSeconds, bool IsEventRepeated = true)
		{
			Timer.Elapsed += elapsedEvent;
			Timer.AutoReset = IsEventRepeated;
			if (intervalSeconds.HasValue)
			{
				Timer.Interval = intervalSeconds.Value * 1000;
			}
		}

		public void EnableTimer()
		{
			Timer.Enabled = true;
		}

		public void DisableTimer()
		{
			Timer.Enabled = false;
		}

		public bool IsTimerStop => !Timer.Enabled;

	}
}
