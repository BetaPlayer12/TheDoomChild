using Holysoft.Event;

namespace DChild.Gameplay.Systems
{
    public struct CurrencyUpdateEventArgs : IEventActionArgs
    {
        public CurrencyUpdateEventArgs(int amount) : this()
        {
            this.amount = amount;
        }

        public int amount { get; }
    }

    public interface ICurrency
    {
        int amount { get; }
        event EventAction<CurrencyUpdateEventArgs> OnAmountSet;
        event EventAction<CurrencyUpdateEventArgs> OnAmountAdded;
    }
}
