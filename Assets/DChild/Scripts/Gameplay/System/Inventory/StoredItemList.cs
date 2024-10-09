using DChild.Gameplay.Items;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public abstract class StoredItemList : IInventory
    {
        [System.Serializable]
        public class StoredItem : IStoredItem
        {
            [SerializeField]
            protected ItemData m_data;
            [SerializeField, MinValue(0), DisableIf("m_hasInfiniteCount"), HorizontalGroup("Count"), HideLabel, MaxValue("@m_data.quantityLimit"), HideIf("@m_data == null")]
            protected int m_count = 1;
            [SerializeField, LabelText("Infinite"), ToggleLeft, HorizontalGroup("Count"), HideIf("@m_data == null")]
            protected bool m_hasInfiniteCount;

            public StoredItem(ItemData data, int count)
            {
                m_data = data;
                m_count = count;
            }

            public ItemData data => m_data;
            public virtual int count => m_hasInfiniteCount ? 99 : m_count;
            public bool hasInfiniteCount => m_hasInfiniteCount;

            public virtual void SetCount(int count)
            {
                if (m_hasInfiniteCount == false)
                {
                    m_count = count;
                }
            }

            public void SetCountToInfinite(bool isInfinite)
            {
                m_hasInfiniteCount = isInfinite;
            }
        }

        public abstract int storedItemCount { get; }

        public event EventAction<ItemEventArgs> InventoryItemUpdate;
        public event EventAction<EventActionArgs> MassInventoryItemUpdate;

        public abstract void AddItem(ItemData itemData, int count = 1);
        public abstract void RemoveItem(ItemData itemData, int count = 1);
        public abstract void SetItem(ItemData itemData, int count = 1);

        public abstract void SetItemAsInfinite(ItemData data, bool isInfinite);

        public abstract IStoredItem[] FindStoredItemsOfType(ItemCategory category);

        public abstract IStoredItem GetItem(int index);
        public abstract IStoredItem GetItem(ItemData itemData);

        public void InvokeMassInventoryItemUpdate() => MassInventoryItemUpdate?.Invoke(this, EventActionArgs.Empty);

        protected void InvokeInventoryItemUpdate(ItemData data, int currentCount, int countModification)
        {
            if (InventoryItemUpdate != null)
            {
                using (Cache<ItemEventArgs> cachedEvent = Cache<ItemEventArgs>.Claim())
                {
                    cachedEvent.Value.Initialize(data, currentCount, countModification);
                    InventoryItemUpdate.Invoke(this, cachedEvent);
                    cachedEvent.Release();
                }
            }
        }
    }

    [System.Serializable]
    public abstract class StoredItemList<T> : StoredItemList where T : StoredItemList.StoredItem
    {
        [SerializeField, PropertyOrder(100), TableList]
        protected List<T> m_items;

        protected StoredItemList()
        {
            m_items = new List<T>();
        }

        public override int storedItemCount => m_items.Count;

        protected abstract T CreateNewStoredItem(ItemData itemData, int count);

        public override IStoredItem GetItem(int index) => m_items[index];
        public override IStoredItem GetItem(ItemData itemData)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                var item = m_items[i];
                if (item.data == itemData)
                {
                    return item;
                }
            }
            return null;
        }

        public override IStoredItem[] FindStoredItemsOfType(ItemCategory category) => m_items.Where((x) => category.HasFlag(x.data.category)).ToArray();

        public override void AddItem(ItemData itemData, int count = 1)
        {
            AddItem(itemData, out T storedItem, count);
            InvokeInventoryItemUpdate(itemData, storedItem.count, count);
        }

        public virtual void AddItem(ItemData itemData, out T storedItem, int count = 1)
        {
            if (TryGetStoredItem(itemData, out storedItem) == false)
            {
                storedItem = CreateNewStoredItem(itemData, count);
                m_items.Add(storedItem);
            }
            else
            {
                storedItem.SetCount(storedItem.count + count);
            }
        }

        public override void RemoveItem(ItemData itemData, int count = 1)
        {
            if (TryGetStoredItem(itemData, out T storedItem))
            {
                storedItem.SetCount(storedItem.count - count);
                if (storedItem.count <= 0)
                {
                    m_items.Remove(storedItem);
                }
                InvokeInventoryItemUpdate(itemData, storedItem.count, -count);
            }
        }

        public override void SetItem(ItemData itemData, int count = 1)
        {
            T storedItem;
            if (TryGetStoredItem(itemData, out storedItem) == false)
            {
                storedItem = CreateNewStoredItem(itemData, count);
                m_items.Add(storedItem);
            }
            else
            {
                storedItem.SetCount(count);
            }
            InvokeInventoryItemUpdate(itemData, storedItem.count, count);
        }

        public override void SetItemAsInfinite(ItemData itemData, bool isInfinite)
        {
            T storedItem;
            if (TryGetStoredItem(itemData, out storedItem) == false)
            {
                storedItem = CreateNewStoredItem(itemData, 99);
                m_items.Add(storedItem);
            }
            storedItem.SetCountToInfinite(isInfinite);
            InvokeInventoryItemUpdate(itemData, storedItem.count, 99);
        }

        public void ClearList()
        {
            m_items.Clear();
        }

        public T GetStoredItem(ItemData itemData)
        {
            for (int i = 0; i < storedItemCount; i++)
            {
                var item = m_items[i];
                if (item.data == itemData)
                    return item;
            }
            return null;
        }

        protected bool TryGetStoredItem(ItemData data, out T storedItem)
        {
            storedItem = GetStoredItem(data);
            return storedItem != null;
        }
    }
}