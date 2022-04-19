using DChild.Gameplay.Items;
using Holysoft.Event;

namespace DChild.Gameplay.Trade
{
    public interface ITradeTransactionInfo
    {
        event EventAction<EventActionArgs> TransactionModified;
        ItemData item { get; }
        int count { get; }
        int totalCost { get; }
    }
}