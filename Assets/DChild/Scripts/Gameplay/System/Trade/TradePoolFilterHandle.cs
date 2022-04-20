using DChild.Gameplay.Items;
using DChild.Gameplay.Trade;
using Holysoft.UI;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class TradePoolFilterHandle : MonoBehaviour
    {
        [SerializeField]
        private TradeManager m_manager;
        [SerializeField]
        private SingleFocusHandler m_singleFocus;
        [SerializeField]
        private TextMeshProUGUI m_filterAppliedLabel;

        public void ResetFilters()
        {
            m_singleFocus.DontFocusOnAnything();
            m_manager.ResetTradeUI();
            m_filterAppliedLabel.text = "";
        }

        public void SetFilter(ItemCategory category,string label)
        {
            m_filterAppliedLabel.text = label;
            m_manager.SetTradeFilter(category);
        }
    }
}