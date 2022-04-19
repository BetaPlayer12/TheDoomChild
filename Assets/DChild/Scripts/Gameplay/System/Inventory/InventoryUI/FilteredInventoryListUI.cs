using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories.UI
{
    public abstract class FilteredInventoryListUI<T> : InventoryListUI<T>
    {
        protected ItemCategory m_currentFilter = ItemCategory.All;

        public void SetFilter(ItemCategory itemCategory)
        {
            if(m_currentFilter != itemCategory)
            {
                m_currentFilter = itemCategory;
                UpdateUIList();
            }
        }

        public void ResetFilter()
        {
            SetFilter(ItemCategory.All);
        }
    }
}