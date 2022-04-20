namespace DChild.Gameplay.Inventories
{
    public interface ITradeItem : IStoredItem
    {
        int cost { get; }
        void OverrideCost(int newCost);
        void RemoveCostOverride();
    }
}