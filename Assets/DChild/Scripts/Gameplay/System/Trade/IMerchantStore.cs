using DChild.Gameplay.Inventories;

namespace DChild.Menu.Trade
{
    public interface IMerchantStore
    {
        void SetWaresReference(InventoryData inventory);
        void ResetWares();
    }
}