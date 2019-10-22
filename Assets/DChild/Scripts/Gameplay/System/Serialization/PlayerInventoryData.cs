﻿using UnityEngine;
#if UNITY_EDITOR
#endif


namespace DChild.Serialization
{

    [System.Serializable]
    public class PlayerInventoryData
    {
     
        [SerializeField]
        private int m_soulEssence;
        [SerializeField]
        private ItemContainerSaveData m_items;
        [SerializeField]
        private ItemContainerSaveData m_soulCrystals;
        [SerializeField]
        private ItemContainerSaveData m_questItems;

        public PlayerInventoryData(int m_soulEssence, ItemContainerSaveData m_items, ItemContainerSaveData m_soulCrystals, ItemContainerSaveData m_questItems)
        {
            this.m_soulEssence = m_soulEssence;
            this.m_items = m_items;
            this.m_soulCrystals = m_soulCrystals;
            this.m_questItems = m_questItems;
        }

        public int soulEssence { get => m_soulEssence; }
        public ItemContainerSaveData items { get => m_items;  }
        public ItemContainerSaveData soulCrystals { get => m_soulCrystals;}
        public ItemContainerSaveData questItems { get => m_questItems; }
    }
}