using DChild.Gameplay.Inventories;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holysoft.Event;
using System;
using Sirenix.Serialization.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Trade.UI
{
    public class TradeSelectionUI : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnSelectionTemplateUpdate"), BoxGroup("ItemSelection")]
        private GameObject m_itemSelectionTemplate;
        [SerializeField, OnValueChanged("OnSelectionTemplateUpdate"), BoxGroup("ItemSelection")]
        private Transform m_tradeItemsParent;
        [SerializeField, MinValue(1), OnValueChanged("OnSelectionTemplateUpdate"), BoxGroup("ItemSelection")]
        private int m_maxItemSelectionCount = 1;
        [SerializeField, HideInInspector]
        private List<TradeSelectionItemButton> m_selection;

        [SerializeField, MinValue(1)]
        private int m_viewIteration = 1;

        public event EventAction<SelectedItemEventArgs> ItemSelected;

        private int m_viewIndex = 0;
        private int m_maxViewIndex = 0;
        private List<ITradeItem> m_selectionReference;

        public void DisplaySelection(ITradeItem[] items)
        {
            m_selectionReference.Clear();
            m_maxViewIndex = (items.Length / m_viewIteration) - 1;
            m_selectionReference.AddRange(items);
            ResetDisplayIndex();

            using (Cache<SelectedItemEventArgs> selectedItemEvent = Cache<SelectedItemEventArgs>.Claim())
            {
                selectedItemEvent.Value.Set(null);
                ItemSelected?.Invoke(this, selectedItemEvent.Value);
                selectedItemEvent.Release();
            }
        }

        public void ResetDisplayIndex()
        {
            m_viewIndex = 0;
            DisplayLimitedSelection();
        }

        public void NextDisplay()
        {
            if (m_viewIndex < m_maxViewIndex)
            {
                m_viewIndex++;
                DisplayLimitedSelection();
            }
        }

        public void PreviousDisplay()
        {
            if (m_viewIndex > 0)
            {
                m_viewIndex--;
                DisplayLimitedSelection();
            }
        }

        private void DisplayLimitedSelection()
        {
            var startingIndexToDisplay = m_viewIteration * m_viewIndex;
            var referenceSize = m_selectionReference.Count - 1;
            var displayableSelectionCount = referenceSize - startingIndexToDisplay;
            var selectionIndex = 0;
            for (; selectionIndex < displayableSelectionCount; selectionIndex++)
            {
                var referenceIndex = startingIndexToDisplay + selectionIndex;
                m_selection[selectionIndex].DisplayItem(m_selectionReference[referenceIndex]);
            }

            for (; selectionIndex < m_selection.Count; selectionIndex++)
            {
                m_selection[selectionIndex].DisplayItem(null);
            }
        }

        private void OnItemSelected(object sender, SelectedItemEventArgs eventArgs)
        {
            ItemSelected?.Invoke(this, eventArgs);
        }

        private void Awake()
        {
            for (int i = 0; i < m_selection.Count; i++)
            {
                m_selection[i].ItemSelected += OnItemSelected;
            }
        }



#if UNITY_EDITOR
        private void OnSelectionTemplateUpdate()
        {
            if (m_selection == null)
            {
                m_selection = new List<TradeSelectionItemButton>();
            }

            for (int i = m_selection.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(m_selection[i].gameObject);
            }
            m_selection.Clear();


            for (int i = 0; i < m_maxItemSelectionCount; i++)
            {
                m_selection.Add(CreateSelectionItem());
            }

            SceneView.RepaintAll();
        }

        private TradeSelectionItemButton CreateSelectionItem()
        {
            return Instantiate(m_itemSelectionTemplate, m_tradeItemsParent).GetComponent<TradeSelectionItemButton>();
        }

        [ContextMenu("ClearCacheList")]
        private void ClearCacheList()
        {
            m_selection.Clear();
        }
#endif
    }

}