using DChild.Gameplay.Items;
using Holysoft.Event;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Trade
{
    [System.Serializable]
    public class TradeTransaction : ITradeTransactionInfo
    {
        [ShowInInspector, OnValueChanged("OnTransactionModified")]
        private ItemData m_item;
        [ShowInInspector, OnValueChanged("OnTransactionModified"), MinValue(0)]
        private int m_costPerItem;
        [ShowInInspector, OnValueChanged("OnTransactionModified"),MinValue(1)]
        private int m_count = 1;

        public event EventAction<EventActionArgs> TransactionModified;

        public ItemData item => m_item;
        public int count => m_count;
        public int totalCost => m_costPerItem * m_count;

        public void SetTransaction(ItemData item, int costPerItem, int count)
        {
            m_item = item;
            m_costPerItem = costPerItem;
            m_count = count;
            TransactionModified?.Invoke(this, EventActionArgs.Empty);
        }

        public void IncreaseItemCount(int increment)
        {
            m_count += increment;
            TransactionModified?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnTransactionModified()
        {
            TransactionModified?.Invoke(this, EventActionArgs.Empty);
        }
    }
}