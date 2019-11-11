using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class MerchantInventory : SerializedMonoBehaviour, ITradableInventory
    {
        [SerializeField]
        private InventoryData m_waresReference;

        [SerializeField, BoxGroup("Inventory")]
        private IItemContainer m_items;

        public int soulEssence => 0;
        public IItemContainer items => m_items;

        public void AddSoulEssence(int value)
        {

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

        private void Start()
        {
            ResetWares();
        }
    }
}
