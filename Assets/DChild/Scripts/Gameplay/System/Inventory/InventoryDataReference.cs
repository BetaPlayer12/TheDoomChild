using DChild.Gameplay.Items;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class InventoryDataReference : IInventoryInfo
    {
        [SerializeField]
        private InventoryData m_inventoryData;

        public int storedItemCount => m_inventoryData.storedItemCount;

        public IStoredItem GetItem(int index) => m_inventoryData.GetItem(index);

        public IStoredItem GetItem(ItemData item) => m_inventoryData.GetItem(item);
    }
}
