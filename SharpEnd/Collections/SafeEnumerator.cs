using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SharpEnd.Collections
{
    public class SafeEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> m_Inner;
        private readonly object m_Lock;

        public SafeEnumerator(Func<IEnumerator<T>> inner, object @lock)
        {
            m_Lock = @lock;
            Monitor.Enter(m_Lock);
            m_Inner = inner();
        }
        
        public bool MoveNext()
        {
            return m_Inner.MoveNext();
        }

        public void Reset()
        {
            m_Inner.Reset();
        }

        public T Current
        {
            get { return m_Inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            Monitor.Exit(m_Lock);
        }
    }
}