using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public class PlayerInventoryUIHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private ItemDetailsUI m_detailedUI;
        [SerializeField]
        private InventoryListUI<IInventory> m_listUI;
        [SerializeField]
        private ItemUI m_firstSelectedItemUI;
        [SerializeField]
        private UsableInventoryItemHandle m_usableInventoryItemHandle;

        public void Select(ItemUI itemUI)
        {
            m_detailedUI.ShowDetails(itemUI.reference);
            if ((itemUI?.reference?.data ?? null) == null || itemUI.reference.data.category != ItemCategory.Consumable)
            {
                m_usableInventoryItemHandle.Hide();
            }
            else
            {
                m_usableInventoryItemHandle.Show();
                m_usableInventoryItemHandle.HandleUsageOfItem(itemUI.reference.data);
            }
        }

        public void Initialize()
        {
            m_listUI.Reset();
            m_listUI.UpdateUIList();
            Select(m_firstSelectedItemUI);
            var button = m_firstSelectedItemUI.GetComponent<UIToggle>();
            button.SetIsOn(true);
        }

        private void OnListOverallChange(object sender, EventActionArgs eventArgs)
        {
            m_detailedUI.ShowDetails(m_firstSelectedItemUI.reference);
        }

        private void OnItemUsedConsumed(object sender, EventActionArgs eventArgs)
        {
            Select(null);
        }

        private void Awake()
        {
            m_listUI.ListOverallChange += OnListOverallChange;
            m_usableInventoryItemHandle.AllItemCountConsumed += OnItemUsedConsumed;
        }
        
    }
}