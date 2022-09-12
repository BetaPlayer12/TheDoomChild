using UnityEngine;

namespace DChild.Gameplay.Trade.UI
{
    public class TradeUI : MonoBehaviour
    {
        [SerializeField]
        private TradeHandle m_tradeHandle;
        [SerializeField]
        private TransactionUI m_transactionUI;

        private void Start()
        {
            m_transactionUI.SetTransaction(m_tradeHandle.transactionInfo);
        }
    }
}
