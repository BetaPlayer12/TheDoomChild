using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public abstract class InventoryListUI<T> : SerializedMonoBehaviour
    {
        [SerializeField]
        protected T m_inventory;
        protected ItemUI[] m_itemUIs;

        public event EventAction<EventActionArgs> ListOverallChange;

        protected int itemUICount => m_itemUIs.Length;

        public void SetInventoryReference(T tradeInventory)
        {
            m_inventory = tradeInventory;
            UpdateUIList();
        }

        public abstract void UpdateUIList();


        protected void InvokeListOverallChange()
        {
            ListOverallChange?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_itemUIs = GetComponentsInChildren<ItemUI>();
        }
        public abstract void Reset();
    }
}