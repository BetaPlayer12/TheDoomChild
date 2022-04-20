using DChild.Gameplay.Inventories;

namespace DChild.Gameplay.Trade
{
    public interface IMerchantStore
    {
        void SetWares(IInventoryInfo inventory);
        void ResetWares();
    }
}