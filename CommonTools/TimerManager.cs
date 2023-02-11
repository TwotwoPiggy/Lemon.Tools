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

		public TimerManager(int intervalSeconds)
		{
			Timer = new Timer(intervalSeconds * 1000);
		}

		public void SetTimer(ElapsedEventHandler elapsedEvent, int? intervalSeconds, bool IsAutoReset = true)
		{
			Timer.Elapsed += elapsedEvent;
			Timer.AutoReset = IsAutoReset;
			if (intervalSeconds.HasValue)
			{
				Timer.Interval = intervalSeconds.Value * 1000;
			}
		}

		public void EnableTimer()
		{
			Timer.Enabled = true;
		}

	}
}
