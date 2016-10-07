using System.Threading;
using System.Threading.Tasks;

namespace SharpEnd.Utility
{
    internal sealed class Delay
    {
        public static void Execute(int millisecondsDelay, ThreadStart action, bool repeat = false)
        {
            new Delay(millisecondsDelay, action, repeat).Start();
        }

        private int m_millisecondsDelay;
        private ThreadStart m_action;
        private bool m_repeat;
        private bool m_running;

        public Delay(int millisecondsDelay, ThreadStart action, bool repeat = false, bool start = true)
        {
            m_millisecondsDelay = millisecondsDelay;
            m_action = action;
            m_repeat = repeat;

            if (!m_repeat && start)
            {
                Start();
            }
        }

        public async void Start()
        {
            m_running = true;

            if (m_repeat)
            {
                while (m_running)
                {
                    await Task.Delay(m_millisecondsDelay);

                    m_action();
                }
            }
            else
            {
                await Task.Delay(m_millisecondsDelay);

                m_action();
            }
        }

        public void Stop()
        {
            m_running = false;
        }
    }
}
