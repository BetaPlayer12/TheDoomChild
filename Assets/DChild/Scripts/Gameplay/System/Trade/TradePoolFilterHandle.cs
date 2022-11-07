using DChild.Gameplay.Items;
using DChild.Gameplay.Trade;
using Doozy.Runtime.UIManager.Components;
using Holysoft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Menu.Trade
{
    public class TradePoolFilterHandle : MonoBehaviour
    {
        [SerializeField]
        private UIToggleGroup m_filterGroup;
        [SerializeField]
        private TradeManager m_manager;
        [SerializeField]
        private TextMeshProUGUI m_filterAppliedLabel;

        public void ResetFilters()
        {
            m_manager.ResetTradeUI();
            m_filterAppliedLabel.text = "";
        }

        public void SetFilter(ItemCategory category, string label)
        {
            m_filterAppliedLabel.text = label;
            m_manager.SetTradeFilter(category);
        }

        private void OnToggleSelected(TradePoolFilterInfo filterButton)
        {
            SetFilter(filterButton.filter, filterButton.filterName);
        }

        private void Start()
        {
            var toggles = m_filterGroup.toggles;
            for (int i = 0; i < toggles.Count; i++)
            {
                var toggle = toggles[i];
                var tradeFilter = toggle.GetComponent<TradePoolFilterInfo>();
                UnityAction action = delegate { OnToggleSelected(tradeFilter); };
                toggle.OnToggleOnCallback.Event.AddListener(action);
                toggle.OnInstantToggleOnCallback.Event.AddListener(action);
            }
        }
    }
}