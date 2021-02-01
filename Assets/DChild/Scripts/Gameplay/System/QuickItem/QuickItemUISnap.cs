using DChild.Gameplay.Inventories;
using Holysoft;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemUISnap : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_handle;

        [SerializeField]
        private QuickItemElement m_chosen;
        [SerializeField]
        private QuickItemElement m_prev;
        [SerializeField]
        private QuickItemElement m_next;

        private void OnItemSelect(object sender, QuickItemHandle.SelectionEventArgs eventArgs)
        {
            UpdateUI(eventArgs.currentIndex);
            //if (eventArgs.selectionType != QuickItemHandle.SelectionEventArgs.SelectionType.None)
            //{
            //}
        }

        private void UpdateUI(int currentIndex)
        {
            var length = m_handle.container.Count;
            m_chosen.Set(m_handle.container.GetSlot(currentIndex));
            if (m_handle.isWrapped)
            {
                var prevIndex = (int)Mathf.Repeat(currentIndex - 1, length);
                m_prev.Set(m_handle.container.GetSlot(prevIndex));
                var nextIndex = (int)Mathf.Repeat(currentIndex + 1, length);
                m_next.Set(m_handle.container.GetSlot(nextIndex));
            }
            else
            {
                m_prev.Set(currentIndex == 0 ? null : m_handle.container.GetSlot(currentIndex - 1));
                m_next.Set(currentIndex == length - 1 ? null : m_handle.container.GetSlot(currentIndex + 1));
            }
        }

        private void Start()
        {
            m_handle.SelectedItem += OnItemSelect;
            UpdateUI(m_handle.currentIndex);
        }
    }
}
