using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
namespace DChild.Gameplay.Inventories
{
    public class UniqueItemContainer : MonoBehaviour, IItemContainer
    {
        [SerializeField]
        private bool m_restrictSize;
        [SerializeField, MinValue(1), ShowIf("m_restrictSize"), ValidateInput("ValidateSize", "Size has exceeded maxSize", InfoMessageType.Error)]
        private int m_maxSize;

        [SerializeField, TableList(ShowIndexLabels = true, NumberOfItemsPerPage = 5, ShowPaging = true),
        ValidateInput("ValidateList", "There are duplicate ItemData or Size has exceeded maxSize", InfoMessageType.Error, IncludeChildren = true), HideReferenceObjectPicker]
        private List<ItemSlot> m_list = new List<ItemSlot>();

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
                    m_list[info.index].AddCount(count);
                    m_list[info.index].RestrictCount();
                    if (m_list[info.index].count == 0)
                    {
                        m_list.RemoveAt(info.index);
                    }
                }
                else
                {
                    var willAddNewSlot = m_restrictSize && IsFull() ? false : true;
                    if (willAddNewSlot)
                    {
                        AddNewSlot(item, count);
                    }
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
                }
                else
                {
                    m_list[info.index].SetCount(count);
                    m_list[info.index].RestrictCount();
                }
            }
            else if (count > 0)
            {
                var willAddNewSlot = m_restrictSize && IsFull() ? false : true;
                if (willAddNewSlot)
                {
                    AddNewSlot(item, count);
                }
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


        public void SetList(ItemContainerData data)
        {
            m_list.Clear();
            m_list.AddRange(data.list);
        }

        private void AddNewSlot(ItemData item, int count)
        {
            var slot = new ItemSlot(item, count);
            slot.RestrictCount();
            m_list.Add(slot);
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
