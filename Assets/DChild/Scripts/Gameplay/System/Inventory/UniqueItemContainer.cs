
using DChild.Gameplay.Items;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Inventories
{
    public class UniqueItemContainer : MonoBehaviour, IItemContainer
    {
        [System.Serializable]
        public class RestrictionInfo
        {
            [SerializeField]
            private ItemData m_itemData;
            [SerializeField]
            private ItemSlot.Restriction m_restriction;

            public ItemData itemData => m_itemData;
            public ItemSlot.Restriction restriction => m_restriction;
        }

        [SerializeField]
        private bool m_restrictSize;
        [SerializeField, MinValue(1), ShowIf("m_restrictSize"), ValidateInput("ValidateSize", "Size has exceeded maxSize", InfoMessageType.Error)]
        private int m_maxSize;

        [SerializeField, TableList(ShowIndexLabels = true, NumberOfItemsPerPage = 5, ShowPaging = true),
        ValidateInput("ValidateList", "There are duplicate ItemData or Size has exceeded maxSize", InfoMessageType.Error, IncludeChildren = true), TabGroup("Items")]
        private List<ItemSlot> m_list = new List<ItemSlot>();
        [SerializeField, TableList(ShowIndexLabels = true, NumberOfItemsPerPage = 5, ShowPaging = true), TabGroup("Restriction")]
        private List<RestrictionInfo> m_restrictions = new List<RestrictionInfo>();

        public event EventAction<ItemEventArgs> ItemUpdate;

        private bool m_hasUpdatedSlot;

        public bool restrictSize => m_restrictSize;
        public int MaxSize => m_maxSize;
        public int Count => m_list.Count;

        public ItemSlot GetSlot(int index)
        {
            if (index < m_list.Count)
            {
                return m_list[index];
            }
            else
            {
                return null;
            }
        }

        public void AddItem(ItemData item, int count)
        {
            if (count != 0)
            {
                var info = GetInfoOf(item);
                if (info.isContainedInList)
                {
                    m_hasUpdatedSlot = true;
                    m_list[info.index].AddCount(count);
                    m_list[info.index].RestrictCount();
                    if (m_list[info.index].count == 0)
                    {
                        m_list.RemoveAt(info.index);
                        SendItemUpdateEvent(item, 0);
                    }
                    else
                    {
                        SendItemUpdateEvent(item, m_list[info.index].count);
                    }
                    m_hasUpdatedSlot = false;
                }
                else if (count > 0)
                {
                    var willAddNewSlot = m_restrictSize && IsFull() ? false : true;
                    if (willAddNewSlot)
                    {
                        AddNewSlot(item, count);
                        SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
                    }
                }
            }
        }

        public void SetItem(ItemData item, int count)
        {
            var info = GetInfoOf(item);
            if (info.isContainedInList)
            {
                m_hasUpdatedSlot = true;
                if (count == 0)
                {
                    m_list.RemoveAt(info.index);
                    SendItemUpdateEvent(item, 0);
                }
                else
                {
                    m_list[info.index].SetCount(count);
                    m_list[info.index].RestrictCount();
                    SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
                }
                m_hasUpdatedSlot = false;
            }
            else if (count > 0)
            {
                var willAddNewSlot = m_restrictSize && IsFull() ? false : true;
                if (willAddNewSlot)
                {
                    AddNewSlot(item, count);
                }
                SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
            }

        }

        private bool FindRestrictions(ItemData item, out ItemSlot.Restriction restriction)
        {
            RestrictionInfo restrictionInfo = null;
            for (int i = 0; i < m_restrictions.Count; i++)
            {
                restrictionInfo = m_restrictions[i];
                if (restrictionInfo.itemData == item)
                {
                    restriction = restrictionInfo.restriction;
                    return true;
                }
            }
            restriction = new ItemSlot.Restriction();
            return false;
        }

        private void SendItemUpdateEvent(ItemData item, int count)
        {
            using (Cache<ItemEventArgs> eventArgs = Cache<ItemEventArgs>.Claim())
            {
                eventArgs.Value.Initialize(item, count);
                ItemUpdate?.Invoke(this, eventArgs);
                eventArgs.Release();
            }
        }

        public bool HasSpaceFor(ItemData item)
        {
            var info = GetInfoOf(item);
            if (info.isContainedInList)
            {
                return m_list[info.index].count < item.quantityLimit;
            }
            else
            {
                return true;
            }
        }

        public int GetCurrentAmount(ItemData item)
        {
            var info = GetInfoOf(item);
            if (info.isContainedInList)
            {
                return GetSlot(info.index).count;
            }
            else
            {
                return 0;
            }
        }

        public void SetList(ItemContainerData data)
        {
            m_list.Clear();
            m_list.AddRange(data.list);
        }

        public ItemContainerSaveData Save()
        {
            List<ItemContainerSaveData.Item> savedData = new List<ItemContainerSaveData.Item>();
            for (int i = 0; i < m_list.Count; i++)
            {
                savedData.Add(new ItemContainerSaveData.Item(m_list[i].item.id, m_list[i].count));
            }
            return new ItemContainerSaveData(savedData.ToArray());
        }

        public bool HasItemCategory(ItemCategory category)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                if (m_list[i].item.category == category)
                {
                    return true;
                }
            }

            return false;
        }

        public void ClearList()
        {
            m_list.Clear();
        }

        private void AddNewSlot(ItemData item, int count)
        {
            var slot = new ItemSlot(item, count);
            slot.RestrictCount();
            if (FindRestrictions(item, out ItemSlot.Restriction restriction))
            {
                slot.SetRestriction(restriction);
            }
            slot.CountChange += OnSlotCountChange;
            m_list.Add(slot);
        }

        private void OnSlotCountChange(object sender, ItemSlot.InfoChangeEventArgs eventArgs)
        {
            if (m_hasUpdatedSlot)
            {
                //Skip
            }
            else
            {
                var itemSlot = (ItemSlot)sender;
                if (eventArgs.count <= 0)
                {
                    m_list.Remove(itemSlot);
                    SendItemUpdateEvent(itemSlot.item, 0);
                }
                else
                {
                    SendItemUpdateEvent(itemSlot.item, itemSlot.count);
                }
            }
        }

        private (bool isContainedInList, int index) GetInfoOf(ItemData data)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                if (m_list[i].item == data)
                {
                    return (true, i);
                }
            }

            return (false, -1);
        }

        private bool IsFull()
        {
            if (m_restrictSize)
            {
                return m_list.Count >= m_maxSize;
            }
            return false;
        }

#if UNITY_EDITOR

        private bool ValidateSize(int size)
        {
            return m_list.Count <= m_maxSize;
        }

        private bool ValidateList(List<ItemSlot> newSlot)
        {
            if (m_restrictSize && m_list.Count > m_maxSize)
            {
                return false;
            }

            List<ItemData> trackedItemData = new List<ItemData>();
            for (int i = 0; i < m_list.Count; i++)
            {
                if (trackedItemData.Contains(m_list[i].item))
                {
                    return false;
                }
                else
                {
                    trackedItemData.Add(m_list[i].item);
                }
            }
            return true;
        }
#endif
    }
}
