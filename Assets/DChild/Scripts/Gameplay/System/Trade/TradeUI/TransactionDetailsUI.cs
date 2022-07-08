using Holysoft.Event;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Trade.UI
{
    public class TransactionDetailsUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_amountToTrade;

        private ITradeTransactionInfo m_transactionInfo;

        public void SetTransactionReference(ITradeTransactionInfo transactionInfo)
        {
            m_transactionInfo = transactionInfo;
            m_transactionInfo.TransactionModified += OnTransactionModified;
            m_amountToTrade.text = m_transactionInfo.count.ToString();
        }

        private void OnTransactionModified(object sender, EventActionArgs eventArgs)
        {
            m_amountToTrade.text = m_transactionInfo.count.ToString();
        }
    }
}
