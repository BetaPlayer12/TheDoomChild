using DChild.Gameplay.Items;
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
        }

        public void AddItem(ItemData item, int count)
        {
            if (count != 0 && (count < 0 || m_allowBuyBack))
            {
                m_items.AddItem(item, count);
            }
        }

        public int GetCurrentAmount(ItemData itemData) => m_items.GetCurrentAmount(itemData);

        public bool CanAfford(int cost)
        {
            return true;
        }

        private void Start()
        {
            ResetWares();
        }

    }
}
