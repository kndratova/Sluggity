using System;
using System.Windows.Threading;

namespace Sluggity.Core
{
    internal class TimerManager
    {
        private const int TimerInterval = 16;
        private static DispatcherTimer _gameTimer;
        private static DateTime _startTime;
        private static TimeSpan _elapsedTime;

        public static event EventHandler Update
        {
            add => _gameTimer.Tick += value;
            remove => _gameTimer.Tick -= value;
        }

        public static void StartTimer()
        {
            _gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(TimerInterval) };
            _gameTimer.Tick += GameTimer_Tick;
            _startTime = DateTime.Now;
            _gameTimer.Start();
        }

        private static void GameTimer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = DateTime.Now - _startTime;
        }

        public static string GetElapsedTime()
        {
            return "[" + $"{_elapsedTime.Minutes:D2}:{_elapsedTime.Seconds:D2}" + "] ";
        }
    }
}