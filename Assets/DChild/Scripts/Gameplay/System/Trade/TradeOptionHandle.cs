using Holysoft.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trade
{
    public class TradeOptionHandle : MonoBehaviour
    {
        private TradeType m_tradeType;
        [SerializeField]
        private Button m_buyButton;
        [SerializeField]
        private Button m_sellButton;
        [SerializeField]
        private TextMeshProUGUI m_tradeButtonLabel;

        private UIHighlightHandler m_buyButtonHighlight;
        private UIHighlightHandler m_sellButtonHighlight;

        public TradeType tradeType => m_tradeType;

        public void ChangeToBuyOption(bool instant)
        {
            m_tradeType = TradeType.Buy;
            m_buyButton.interactable = false;
            m_sellButton.interactable = true;
            m_tradeButtonLabel.text = "Buy";
            if (instant)
            {
                m_buyButtonHighlight.UseHighlightState();
                m_sellButtonHighlight.UseNormalizeState();
            }
            else
            {
                m_buyButtonHighlight.Highlight();
                m_sellButtonHighlight.Normalize();
            }
        }
        public void ChangeToSellOption(bool instant)
        {
            m_tradeType = TradeType.Sell;
            m_buyButton.interactable = true;
            m_sellButton.interactable = false;
            m_tradeButtonLabel.text = "Sell";
            if (instant)
            {
                m_buyButtonHighlight.UseNormalizeState();
                m_sellButtonHighlight.UseHighlightState();
            }
            else
            {
                m_buyButtonHighlight.Normalize();
                m_sellButtonHighlight.Highlight();
            }
        }

        private void Awake()
        {
            m_buyButtonHighlight = m_buyButton.GetComponent<UIHighlightHandler>();
            m_sellButtonHighlight = m_sellButton.GetComponent<UIHighlightHandler>();
        }
    }
}