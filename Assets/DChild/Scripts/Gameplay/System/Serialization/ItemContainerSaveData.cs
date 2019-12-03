﻿using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif


namespace DChild.Serialization
{
    [System.Serializable]
    public class ItemContainerSaveData
    {
        [System.Serializable]
        public struct Item
        {
            [SerializeField]
            private int m_ID;
            [SerializeField]
            private int m_count;

            public Item(int m_ID, int m_count)
            {
                this.m_ID = m_ID;
                this.m_count = m_count;
            }

            public int ID => m_ID;
            public int count => m_count;
        }

        [SerializeField]
        private Item[] m_datas;

        public ItemContainerSaveData()
        {
            m_datas = new Item[0];
        }

        public ItemContainerSaveData(Item[] m_datas)
        {
            this.m_datas = m_datas;
        }

        public Item[] datas { get => m_datas;  }

#if UNITY_EDITOR
        public ItemContainerSaveData(ItemContainerSaveData m_datas)
        {
            List<Item> itemList = new List<Item>(m_datas.datas);
            this.m_datas = itemList.ToArray();
        }
#endif
    }
}