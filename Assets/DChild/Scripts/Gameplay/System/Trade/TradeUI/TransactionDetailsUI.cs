using Holysoft.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trade.UI
{
    public class TransactionDetailsUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_amountToTrade;

        [SerializeField]
        private Button m_button;

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

            if (m_transactionInfo.count > 0)
            {

                m_button.interactable = true;
            }
            else
            {
                m_button.interactable = false;
            }
        }
    }
}
