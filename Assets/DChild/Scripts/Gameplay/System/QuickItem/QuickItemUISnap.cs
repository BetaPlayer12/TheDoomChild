using DChild.Gameplay.Inventories;
using Holysoft;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemUISnap : MonoBehaviour
    {
        [SerializeField]
        private QuickItemHandle m_handle;
        [SerializeField]
        private QuickItemSelections m_selections;

        [SerializeField]
        private QuickItemElement m_chosen;
        [SerializeField]
        private QuickItemElement m_prev;
        [SerializeField]
        private QuickItemElement m_next;

        private int m_currentIndex;

        private void OnItemSelect(object sender, QuickItemHandle.SelectionEventArgs eventArgs)
        {
            m_currentIndex = eventArgs.currentIndex;
            UpdateUI(eventArgs.currentIndex);
        }

        private void UpdateUI(int currentIndex)
        {
            var length = m_selections.itemCount;
            if (length > 0)
            {
                m_chosen.ShowDetails(m_selections.GetItem(currentIndex));
                if (m_handle.isWrapped)
                {
                    var prevIndex = (int)Mathf.Repeat(currentIndex - 1, length);
                    m_prev.ShowDetails(m_selections.GetItem(prevIndex));
                    var nextIndex = (int)Mathf.Repeat(currentIndex + 1, length);
                    m_next.ShowDetails(m_selections.GetItem(nextIndex));
                }
                else
                {
                    m_prev.ShowDetails(currentIndex == 0 ? null : m_selections.GetItem(currentIndex - 1));
                    m_next.ShowDetails(currentIndex == length - 1 ? null : m_selections.GetItem(currentIndex + 1));
                }
            }
            else
            {
                m_chosen.ShowDetails(null);
                m_prev.ShowDetails(null);
                m_next.ShowDetails(null);
            }
        }

        private void Start()
        {
            m_handle.SelectedItem += OnItemSelect;
            UpdateUI(m_handle.currentIndex);
        }
    }
}
