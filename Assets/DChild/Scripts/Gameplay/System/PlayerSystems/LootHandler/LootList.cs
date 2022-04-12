using UnityEngine;
using System.Collections.Generic;
using DChild.Gameplay.Items;
using System.Linq;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Systems
{
    [SerializeField]
    public class LootList
    {
        private Dictionary<ItemData, int> m_lootAmountPair;

        public LootList()
        {
            m_lootAmountPair = new Dictionary<ItemData, int>();
        }

        public void Add(ItemData item, int count)
        {
            if (m_lootAmountPair.ContainsKey(item))
            {
                var newCount = m_lootAmountPair[item] + count;
                if (newCount <= 0)
                {
                    m_lootAmountPair.Remove(item);
                }
                else
                {
                    m_lootAmountPair[item] = newCount;
                }
            }
            else if (count > 0)
            {
                m_lootAmountPair.Add(item, count);
            }
        }

        public ItemData[] GetAllItems() => m_lootAmountPair.Keys.ToArray();

        public int GetCountOf(ItemData itemData)
        {
            m_lootAmountPair.TryGetValue(itemData, out int count);
            return count;
        }

        public void Clear()
        {
            m_lootAmountPair.Clear();
        }
    }
}