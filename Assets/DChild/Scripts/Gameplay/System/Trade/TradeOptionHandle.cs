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
        private TextMeshProUGUI m_tradeButtonLabel;

        public TradeType tradeType => m_tradeType;

        public void ChangeToBuyOption(bool instant)
        {
            m_tradeType = TradeType.Buy;
            m_tradeButtonLabel.text = "Buy";
        }
        public void ChangeToSellOption(bool instant)
        {
            m_tradeType = TradeType.Sell;
            m_tradeButtonLabel.text = "Sell";
        }
    }
}