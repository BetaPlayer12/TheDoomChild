using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public abstract class CountdownArray<T> where T : class
    {
        public struct ItemEventArgs<U> : IEventActionArgs
        {
            public ItemEventArgs(U item, int index)
            {
                this.item = item;
                this.index = index;
            }

            public U item { get; private set; }
            public int index { get; private set; }
        }

        public event EventAction<ItemEventArgs<T>> ItemEnd;

        [SerializeField]
        [MinValue(0.1f)]
        protected float m_duration;

        protected List<T> m_items;
        protected List<float> m_timers;

        protected bool m_canUpdate;

        public void Initialize()
        {
            m_items = new List<T>();
            m_timers = new List<float>();
        }

        public void ResetTimerOf(int index) => m_timers[index] = m_duration;

        public void Add(T item)
        {
            if (m_items.Contains(item) == false)
            {
                m_items.Insert(0, item);
                m_timers.Insert(0, m_duration);
                m_canUpdate = true;
            }
        }

        public void Remove(T item)
        {

            if (m_items.Contains(item))
            {
                var index = m_items.FindIndex(a => a == item);
                m_items.RemoveAt(index);
                m_timers.RemoveAt(index);
                if (m_items.Count == 0)
                {
                    m_canUpdate = false;
                }
            }
        }

        public void Update(float deltaTime)
        {
            if (m_canUpdate)
            {
                for (int i = m_timers.Count - 1; i >= 0; i--)
                {
                    m_timers[i] -= deltaTime;
                    if (m_timers[i] <= 0)
                    {
                        ItemEnd?.Invoke(this, new ItemEventArgs<T>(m_items[i], i));
                    }
                }
            }
        }
    }

}