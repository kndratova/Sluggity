using System;
using System.Diagnostics;
using System.Windows.Media;

namespace Sluggity.Core
{
    public class GameLoop
    {
        public const double FixedDeltaTime = 1.0 / 60;
        private const double k_maxFrameTime = 0.25;

        private readonly Stopwatch m_stopwatch = new Stopwatch();
        private double m_lastTime;
        private double m_accumulator;

        public event Action<double> Update;
        public event Action<double> FixedUpdate;
        public event Action<double> Render;

        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            m_lastTime = 0;
            m_accumulator = 0;
            m_stopwatch.Restart();
            CompositionTarget.Rendering += OnRendering;
        }

        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }

            IsRunning = false;
            m_stopwatch.Stop();
            CompositionTarget.Rendering -= OnRendering;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            double time = m_stopwatch.Elapsed.TotalSeconds;
            double frameTime = Math.Min(time - m_lastTime, k_maxFrameTime);
            m_lastTime = time;

            Update?.Invoke(frameTime);

            m_accumulator += frameTime;
            while (m_accumulator >= FixedDeltaTime)
            {
                FixedUpdate?.Invoke(FixedDeltaTime);
                m_accumulator -= FixedDeltaTime;
            }

            Render?.Invoke(frameTime);
        }
    }
}