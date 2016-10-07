using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpEnd.Threading
{
    internal sealed class Delay
    {
        private int m_delay;
        private Action m_action;
        private bool m_repeat;
        private bool m_running = true;

        public static void Execute(int delay, Action action) => new Delay(delay, action).Execute();

        public Delay(int delay, Action action, bool repeat = false)
        {
            m_delay = delay;
            m_action = action;
            m_repeat = repeat;
        }

        public async void Execute()
        {
            if (m_repeat)
            {
                while (m_running)
                {
                    await Task.Delay(m_delay);

                    m_action();
                }
            }
            else
            {
                await Task.Delay(m_delay);

                m_action();
            }
        }

        public void Cancel()
        {
            m_running = false;
        }
    }
}
