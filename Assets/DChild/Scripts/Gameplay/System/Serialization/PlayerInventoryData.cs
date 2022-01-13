using UnityEngine;
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

        //MiguelTest
        [SerializeField]
        private ItemContainerSaveData m_soulSkills;

        public PlayerInventoryData()
        {
            m_soulEssence = 0;
            this.m_items = new ItemContainerSaveData();
            this.m_soulCrystals = new ItemContainerSaveData();
            this.m_questItems = new ItemContainerSaveData();
            //MiguelTest
            this.m_soulSkills = new ItemContainerSaveData();
        }

        public PlayerInventoryData(int m_soulEssence, ItemContainerSaveData m_items, ItemContainerSaveData m_soulCrystals, ItemContainerSaveData m_questItems, ItemContainerSaveData m_soulSkills)
        {
            this.m_soulEssence = m_soulEssence;
            this.m_items = m_items;
            this.m_soulCrystals = m_soulCrystals;
            this.m_questItems = m_questItems;
            //MiguelTest
            this.m_soulSkills = m_soulSkills;
        }

        public PlayerInventoryData(PlayerInventoryData data)
        {
            this.m_soulEssence = data.soulEssence;
            this.m_items = new ItemContainerSaveData(data.items);
            this.m_soulCrystals = new ItemContainerSaveData(data.soulCrystals);
            this.m_questItems = new ItemContainerSaveData(data.questItems);

            //MiguelTest
            this.m_soulSkills = new ItemContainerSaveData(data.m_soulSkills);   
        }

        public int soulEssence { get => m_soulEssence; }
        public ItemContainerSaveData items { get => m_items;  }
        public ItemContainerSaveData soulCrystals { get => m_soulCrystals;}
        public ItemContainerSaveData questItems { get => m_questItems; }
        //MiguelTest
        public ItemContainerSaveData soulSkills { get => m_soulSkills; }
    }
}