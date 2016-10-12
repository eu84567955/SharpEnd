using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SharpEnd.Threading
{
    internal sealed class Delay
    {
        public static void Execute(double delay, Action action)
        {
            new Delay(delay, action).Execute();
        }

        private Timer m_timer;

        public Delay(double delay, Action action, bool repeat = false)
        {
            m_timer = new Timer(delay);

            m_timer.Elapsed += new ElapsedEventHandler((o, e) =>
            {
                action();

                if (!repeat)
                {
                    Cancel();
                }
            });

            m_timer.Start();
        }

        public void Execute()
        {
            m_timer.Start();
        }

        public void Cancel()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer.Dispose();
            }

            m_timer = null;
        }
    }
}
