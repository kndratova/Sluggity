using System;
using System.Diagnostics;
using System.Windows.Media;

namespace Sluggity.Core
{
    public class GameLoop
    {
        private const double k_maxDeltaTime = 1.0 / 30;

        private readonly Stopwatch m_stopwatch = new Stopwatch();
        private double m_lastTime;
        private bool m_isRunning;

        public event Action<double> Tick;

        public bool IsRunning => m_isRunning;
    
        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            m_isRunning = true;
            m_lastTime = 0;
            m_stopwatch.Restart();
            CompositionTarget.Rendering += OnRendering;
        }
    
        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }

            m_isRunning = false;
            m_stopwatch.Stop();
            CompositionTarget.Rendering -= OnRendering;
        }
    
        private void OnRendering(object sender, EventArgs e)
        {
            double time = m_stopwatch.Elapsed.TotalSeconds;
            double deltaTime = Math.Min(time - m_lastTime, k_maxDeltaTime);
            m_lastTime = time;

            Tick?.Invoke(deltaTime);
        }
    }
}