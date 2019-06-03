using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class StatusEffectPool : ObjectPool
    {
        [SerializeField]
        [MinValue(0.1f)]
        private float m_poolDuration;
        private Dictionary<StatusEffectType, List<StatusEffect>> m_items;
        private List<float> m_timers;
        private List<StatusEffectType> m_itemTypes;
        protected int m_poolCount;

        public StatusEffectPool()
        {
            m_poolDuration = 5;
            m_items = new Dictionary<StatusEffectType, List<StatusEffect>>();
            m_timers = new List<float>();
            m_itemTypes = new List<StatusEffectType>();
            m_poolCount = 0;
        }

        public void AddToPool(StatusEffect item)
        {
            item.SetParent(poolItemStorage);
            if (m_items.ContainsKey(item.type))
            {
                m_items[item.type].Add(item);
            }
            else
            {
                var list = new List<StatusEffect>();
                list.Add(item);
                m_items.Add(item.type, list);
            }

            m_timers.Add(m_poolDuration);
            m_itemTypes.Add(item.type);
            m_poolCount++;
        }

        public StatusEffect RetrieveFromPool(StatusEffectType type)
        {
            if (m_items.ContainsKey(type))
            {
                var list = m_items[type];
                if (list.Count > 0)
                {
                    var item = list[0];
                    var index = m_itemTypes.FindIndex(x => x == type);
                    m_itemTypes.RemoveAt(index);
                    m_timers.RemoveAt(index);
                    m_poolCount--;
                    return item;
                }
            }

            return null;
        }


        public override void Clear()
        {
            foreach (var list in m_items.Values)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    list[i].DestroyItem();
                }
            }
        }

        public override void Update(float deltaTime)
        {
            for (int i = m_poolCount - 1; i >= 0; i--)
            {
                m_timers[i] -= deltaTime;
                if (m_timers[i] <= 0)
                {
                    var item = RemoveFromPool(i);
                    item.DestroyItem();
                }
            }
        }

        protected StatusEffect RemoveFromPool(int index)
        {
            var list = m_items[m_itemTypes[index]];
            var item = list[0];
            item.SetParent(null);
            list.RemoveAt(0);
            m_timers.RemoveAt(index);
            m_itemTypes.RemoveAt(index);
            m_poolCount--;
            return item;
        }
    }
}