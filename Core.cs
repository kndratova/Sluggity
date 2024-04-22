using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace Sluggity
{
    internal class Core 
    {
        private const int TimerInterval = 16;
        public static DispatcherTimer GameTimer { get; private set; }

        public Core()
        {
            GameTimer = new DispatcherTimer();
            GameTimer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            GameTimer.Start();

            GameTimer.Tick += WriteTickDebug; 
        }

        private void WriteTickDebug(object sender, EventArgs e)
        {
            ///Debug.WriteLine("Tick");
        }

        ~Core()
        {
            GameTimer.Stop();
            GameTimer.Tick -= WriteTickDebug;
        }
    }    
}
