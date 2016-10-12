namespace SharpEnd.Utility
{
    internal sealed class AutoIncrement
    {
        private object m_lock;

        private int m_current;

        public AutoIncrement(int start = 0)
        {
            m_lock = new object();

            m_current = start;
        }

        public int Next
        {
            get
            {
                lock (m_lock)
                {
                    return ++m_current;
                }
            }
        }
    }
}
