using DChild.Gameplay.Inventories;
using DChild.Gameplay.Inventories.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{

    [AddComponentMenu(PreAlphaUtility.COMPONENTMENU_ADDRESS + "ItemUsageTracker")]
    public class ItemUsageTracker : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class SaveData
        {
            [System.Serializable]
            public class Info
            {
                public string item;
                public int useCount;

                public Info(string item, int useCount)
                {
                    this.item = item;
                    this.useCount = useCount;
                }
            }

            [SerializeField]
            private Info[] m_infos;
            public SaveData(Dictionary<string, int> itemUsedPair)
            {
                m_infos = new Info[itemUsedPair.Count];
                int i = 0;
                foreach (var item in itemUsedPair)
                {
                    m_infos[i] = new Info(item.Key, item.Value);
                    i++;
                }
            }

            public Info[] infos => m_infos;
        }

        [SerializeField]
        private QuickItemHandle m_quickItemHandle;
        [SerializeField]
        private UsableInventoryItemHandle m_usableInventoryItemHandle;
        [SerializeField]
        private Dictionary<string, int> m_itemUsedPair;

        public SaveData Save() => new SaveData(m_itemUsedPair);

        public void Load(SaveData data)
        {
            if (m_itemUsedPair == null)
            {
                m_itemUsedPair = new Dictionary<string, int>();
            }

            m_itemUsedPair.Clear();
            var infos = data.infos;
            foreach (var info in infos)
            {
                m_itemUsedPair.Add(info.item, info.useCount);
            }
        }


        private void Start()
        {
            if (m_itemUsedPair == null)
            {
                m_itemUsedPair = new Dictionary<string, int>();
            }
            m_quickItemHandle.ItemUsed += OnItemUsed;
            m_usableInventoryItemHandle.ItemUsed += OnItemUsed;
        }

        private void OnDestroy()
        {
            m_quickItemHandle.ItemUsed -= OnItemUsed;
            m_usableInventoryItemHandle.ItemUsed -= OnItemUsed;
        }

        private void OnItemUsed(string obj)
        {
            if (m_itemUsedPair.ContainsKey(obj))
            {
                m_itemUsedPair[obj] += 1;
            }
            else
            {
                m_itemUsedPair.Add(obj, 1);
            }
        }
    }

}