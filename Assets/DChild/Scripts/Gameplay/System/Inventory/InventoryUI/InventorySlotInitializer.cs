using Doozy.Runtime.UIManager.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Inventories.UI
{
    public class InventorySlotInitializer : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventoryUIHandle m_handle;
        [SerializeField]
        private UIToggleGroup m_itemGroup;

        private void OnItemSelected(ItemUI tradeFilter)
        {
            m_handle.Select(tradeFilter);
        }

        private void AddToggleOnListener(UIToggle toggle)
        {
            var tradeFilter = toggle.GetComponent<ItemUI>();
            UnityAction action = delegate { OnItemSelected(tradeFilter); };
            toggle.OnToggleOnCallback.Event.AddListener(action);
            toggle.OnInstantToggleOnCallback.Event.AddListener(action);
        }

        private IEnumerator Start()
        {
            while (m_itemGroup.numberOfToggles == 0)
                yield return null;

            var toggles = m_itemGroup.toggles;
            AddToggleOnListener(m_itemGroup.FirstToggle);
            for (int i = 0; i < toggles.Count; i++)
            {
                var toggle = toggles[i];
                AddToggleOnListener(toggle);
            }

            Debug.Log("Inventory Slots Initialized: " + m_itemGroup.numberOfToggles);
        }
    }
}