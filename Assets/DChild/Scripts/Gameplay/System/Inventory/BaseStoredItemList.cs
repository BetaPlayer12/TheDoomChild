using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class BaseStoredItemList : StoredItemList<StoredItemList.StoredItem>
    {
        protected override StoredItem CreateNewStoredItem(ItemData itemData, int count)
        {
            return new StoredItem(itemData, count);
        }
    }
}