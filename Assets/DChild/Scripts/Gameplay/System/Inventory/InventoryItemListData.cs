using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [CreateAssetMenu(fileName = "InventoryItemListData", menuName = "DChild/Gameplay/Items/Inventory Item ListData")]
    public class InventoryItemListData : ScriptableObject
    {
        [SerializeField,HideLabel]
        private BaseStoredItemList m_items;

        public int count => m_items.storedItemCount;

        public IStoredItem GetStoredItem(int index) => m_items.GetItem(index);
    }
}