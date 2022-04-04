using DChild.Gameplay.Inventories;

namespace DChild.Menu.Trading
{
    public interface IMerchantStore
    {
        void SetWaresReference(InventoryData inventory);
        void ResetWares();
    }
}