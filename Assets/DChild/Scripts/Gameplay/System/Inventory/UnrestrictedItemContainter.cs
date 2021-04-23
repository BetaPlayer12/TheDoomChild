
using DChild.Gameplay.Items;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Inventories
{
    public class UnrestrictedItemContainter : MonoBehaviour, IItemContainer
    {
        [SerializeField, TableList(ShowIndexLabels = true, NumberOfItemsPerPage = 5, ShowPaging = true),
        ValidateInput("ValidateList", "There are duplicate ItemData or Size has exceeded maxSize", InfoMessageType.Error, IncludeChildren = true), HideReferenceObjectPicker]
        private List<ItemSlot> m_list = new List<ItemSlot>();

        public event EventAction<ItemEventArgs> ItemUpdate;

        public bool restrictSize => false;
        public int MaxSize => 99999;
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
                    m_list[info.index].AddCount(count);
                    if (m_list[info.index].count == 0)
                    {
                        m_list.RemoveAt(info.index);
                        SendItemUpdateEvent(item, 0);
                    }
                    else
                    {
                        SendItemUpdateEvent(item, m_list[info.index].count);
                    }
                }
                else if (count > 0)
                {
                    AddNewSlot(item, count);
                    SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
                }
            }
        }

        public void SetItem(ItemData item, int count)
        {
            var info = GetInfoOf(item);
            if (info.isContainedInList)
            {
                if (count == 0)
                {
                    m_list.RemoveAt(info.index);
                    SendItemUpdateEvent(item, 0);
                }
                else
                {
                    m_list[info.index].SetCount(count);
                    SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
                }
            }
            else if (count > 0)
            {
                AddNewSlot(item, count);
                SendItemUpdateEvent(item, GetSlot(GetInfoOf(item).index).count);
            }
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

        public bool HasSpaceFor(ItemData item) => true;

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
                //savedData.Add(new ItemContainerSaveData.Item(m_list[i].item.id, m_list[i].count));
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

        public void ClearList() => m_list.Clear();

        private void AddNewSlot(ItemData item, int count) => m_list.Add(new ItemSlot(item, count));

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

#if UNITY_EDITOR
        private bool ValidateList(List<ItemSlot> newSlot)
        {
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
