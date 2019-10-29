namespace DChild.Gameplay.Inventories
{
    public interface ITradableInventory
    {
        int soulEssence { get; }
        IItemContainer items { get; }
    }
}
