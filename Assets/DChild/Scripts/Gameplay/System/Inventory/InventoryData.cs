using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "DChild/Database/Inventory Data")]
    public class InventoryData : ScriptableObject, IInventoryInfo
    {
        [SerializeField,TableList(AlwaysExpanded = true),HideLabel]
        private BaseStoredItemList m_itemList;

        public int storedItemCount => m_itemList.storedItemCount;

        public IStoredItem GetItem(int index) => m_itemList.GetItem(index);

        public IStoredItem GetItem(ItemData item) => m_itemList.GetItem(item);
    }
}
