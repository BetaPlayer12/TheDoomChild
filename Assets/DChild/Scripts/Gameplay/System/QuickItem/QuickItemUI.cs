using DChild.Gameplay.Inventories;
using Holysoft;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemUI : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_handle;

        [SerializeField]
        private QuickItemElement m_chosen;
        [SerializeField]
        private QuickItemElement m_prev;
        [SerializeField]
        private QuickItemElement m_next;

        private void Start()
        {
            m_handle.SelectedItem += OnItemSelect;
            UpdateUI(m_handle.currentIndex);
        }

        private void OnItemSelect(object sender, QuickItemHandle.SelectionEventArgs eventArgs)
        {
            UpdateUI(eventArgs.currentIndex);
        }

        private void UpdateUI(int currentIndex)
        {
            var length = m_handle.container.Count;
            m_chosen.Set(m_handle.container.GetSlot(currentIndex));
            var prevIndex = (int)Mathf.Repeat(currentIndex - 1, length);
            m_prev.Set(m_handle.container.GetSlot(prevIndex));
            var nextIndex = (int)Mathf.Repeat(currentIndex + 1, length);
            m_next.Set(m_handle.container.GetSlot(nextIndex));
        }
    } 
}
