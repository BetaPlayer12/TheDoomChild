using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "ItemRequirement", menuName = "DChild/Gameplay/Item Requirement")]
    public class ItemRequirement : ScriptableObject
    {
        [System.Serializable]
        private class Info
        {
            [SerializeField]
            private ItemData m_item;
            [SerializeField, MinValue(1)]
            private int m_count;
            [SerializeField]
            private bool m_consumeItem;

            public ItemData item => m_item;
            public int count => m_count;
            public bool consumeItem => m_consumeItem;
        }

        [SerializeField,TableList]
        private Info[] m_requirements;

        public bool HasAllItems(IInventory inventory)
        {
            bool hasAllItems = true;
            for (int i = 0; i < m_requirements.Length; i++)
            {
                var requirement = m_requirements[i];
                if (inventory.GetItem(requirement.item).count < requirement.count)
                {
                    hasAllItems = false;
                    break;
                }
            }
            return hasAllItems;
        }

        public void ConsumeItems(IInventory inventory)
        {
            Info info = null;
            for (int i = 0; i < m_requirements.Length; i++)
            {
                info = m_requirements[i];
                if (info.consumeItem)
                {
                    inventory.AddItem(info.item, -info.count);
                }
            }
        }
    }
}