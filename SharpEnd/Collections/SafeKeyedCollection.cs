using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpEnd.Collections
{
    public abstract class SafeKeyedCollection<TKey, TItem> : IEnumerable<TItem>
    {
        private object m_Lock;
        private Dictionary<TKey, TItem> m_Inner;

        public SafeKeyedCollection()
        {
            m_Lock = new object();
            m_Inner = new Dictionary<TKey, TItem>();
        }

        public int Count
        {
            get
            {
                lock (m_Lock)
                {
                    return m_Inner.Count;
                }
            }
        }

        public TItem this[TKey key]
        {
            get
            {
                lock (m_Lock)
                {
                    TItem ret;

                    if (!m_Inner.TryGetValue(key, out ret))
                    {
                        throw new KeyNotFoundException();
                    }

                    return ret;
                }
            }
        }

        public void Add(TItem item)
        {
            InsertItem(item);
        }

        public void Remove(TKey key)
        {
            TItem item = this[key];

            this.Remove(item);
        }

        public void Remove(TItem item)
        {
            this.RemoveItem(item);
        }

        protected virtual void InsertItem(TItem item)
        {
            lock (m_Lock)
            {
                TKey key = GetKeyForItem(item);
                m_Inner.Add(key, item);
            }
        }
        protected virtual void RemoveItem(TItem item)
        {
            lock (m_Lock)
            {
                TKey key = GetKeyForItem(item);
                m_Inner.Remove(key);
            }
        }
        public void Clear()
        {
            lock (m_Lock)
            {
                m_Inner.Clear();
            }
        }
        public bool Contains(TKey key)
        {
            lock (m_Lock)
            {
                return m_Inner.ContainsKey(key);
            }
        }
        public bool Contains(TItem value)
        {
            lock (m_Lock)
            {
                return m_Inner.ContainsValue(value);
            }
        }

        protected abstract TKey GetKeyForItem(TItem item);

        protected virtual void ClearItems() { }

        public IEnumerator<TItem> GetEnumerator()
        {
            return new SafeEnumerator<TItem>(() => m_Inner.Values.GetEnumerator(), m_Lock);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}