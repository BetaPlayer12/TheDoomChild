using DChild.Gameplay.Items;
using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class MerchantInventory : SerializedMonoBehaviour, ITradableInventory
    {
        [SerializeField]
        private InventoryData m_waresReference;

        [SerializeField]
        private int m_soulEssence;
        [SerializeField]
        private bool m_allowBuyBack;
        [SerializeField, BoxGroup("Inventory")]
        private IItemContainer m_items;

        public int soulEssence => m_soulEssence;
        public IItemContainer items => m_items;

        public int Count => m_items.Count;

        public void AddSoulEssence(int value)
        {
            m_soulEssence += value;
        }

        public void SetWaresReference(InventoryData inventory)
        {
            m_waresReference = inventory;
        }

        public void ResetWares()
        {
            m_items.ClearList();
            for (int i = 0; i < m_waresReference.slotCount; i++)
            {
                var slotInfo = m_waresReference.GetInfo(i);
                m_items.SetItem(slotInfo.item, slotInfo.count);
            }

            for (int i = 0; i < m_items.Count; i++)
            {
               var slot =  m_items.GetSlot(i);
                var restriction = new ItemSlot.Restriction
                {
                    canBeSold = true,
                    hasInfiniteCount = slot.restrictions.hasInfiniteCount
                };
                slot.SetRestriction(restriction);
            }
        }

        void ITradableInventory.AddItem(ItemData item, int count)
        {
            if (count > 0)
            {
                if (m_allowBuyBack)
                {
                    if (m_items.GetCurrentAmount(item) < 99)
                    {
                        m_items.AddItem(item, count);
                    }
                }
            }
            else if (count < 0)
            {
                if (m_waresReference.HasLimitedCount(item))
                {
                    m_items.AddItem(item, count);
                }
            }
        }

        public void AddToWares(ItemData item, int count = 1)
        {
            if (count > 0)
            {
                m_items.AddItem(item, count);
            }
        }

        public int GetCurrentAmount(ItemData itemData) => m_items.GetCurrentAmount(itemData);

        public bool CanAfford(int cost) => true;
        public ItemSlot GetSlot(int index) => m_items.GetSlot(index);

        private void Start()
        {
            ResetWares();
        }
    }
}
