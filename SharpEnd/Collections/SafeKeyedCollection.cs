using System.Collections;
using System.Collections.Generic;

namespace SharpEnd.Collections
{
    public abstract class SafeKeyedCollection<TKey, TItem> : IEnumerable<TItem>
    {
        private object m_lock;
        private Dictionary<TKey, TItem> m_inner;

        public SafeKeyedCollection()
        {
            m_lock = new object();
            m_inner = new Dictionary<TKey, TItem>();
        }

        public int Count
        {
            get
            {
                lock (m_lock)
                {
                    return m_inner.Count;
                }
            }
        }

        public TItem this[TKey key]
        {
            get
            {
                lock (m_lock)
                {
                    TItem ret;

                    if (!m_inner.TryGetValue(key, out ret))
                    {
                        throw new KeyNotFoundException();
                    }

                    return ret;
                }
            }
        }

        public bool Contains(TKey key)
        {
            lock (m_lock)
            {
                return m_inner.ContainsKey(key);
            }
        }

        public bool Contains(TItem value)
        {
            lock (m_lock)
            {
                return m_inner.ContainsValue(value);
            }
        }

        public void Add(TItem item)
        {
            InsertItem(item);
        }

        public void Remove(TKey key)
        {
            TItem item = this[key];

            Remove(item);
        }

        public void Remove(TItem item)
        {
            RemoveItem(item);
        }

        public void Clear()
        {
            lock (m_lock)
            {
                m_inner.Clear();
            }
        }

        protected virtual void InsertItem(TItem item)
        {
            lock (m_lock)
            {
                TKey key = GetKeyForItem(item);
                m_inner.Add(key, item);
            }
        }

        protected virtual void RemoveItem(TItem item)
        {
            lock (m_lock)
            {
                TKey key = GetKeyForItem(item);
                m_inner.Remove(key);
            }
        }

        protected abstract TKey GetKeyForItem(TItem item);

        public IEnumerator<TItem> GetEnumerator()
        {
            return new SafeEnumerator<TItem>(() => m_inner.Values.GetEnumerator(), m_lock);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}