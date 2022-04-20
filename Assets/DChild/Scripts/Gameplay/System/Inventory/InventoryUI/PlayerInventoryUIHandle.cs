using DChild.Gameplay.Items;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
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
        private SingleFocusHandler m_highlighter;
        [SerializeField]
        private UsableInventoryItemHandle m_usableInventoryItemHandle;

        public void Select(ItemUI itemUI)
        {
            m_detailedUI.ShowDetails(itemUI.reference);
            if (itemUI == null || itemUI.reference.data.category != ItemCategory.Consumable)
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
        }

        private void OnListOverallChange(object sender, EventActionArgs eventArgs)
        {
            m_detailedUI.ShowDetails(m_firstSelectedItemUI.reference);
        }

        private void OnItemUsedConsumed(object sender, EventActionArgs eventArgs)
        {
            Select(null);
            m_highlighter.DontFocusOnAnything();
        }


        private void Awake()
        {
            m_listUI.ListOverallChange += OnListOverallChange;
            m_usableInventoryItemHandle.AllItemCountConsumed += OnItemUsedConsumed;
        }

    }
}