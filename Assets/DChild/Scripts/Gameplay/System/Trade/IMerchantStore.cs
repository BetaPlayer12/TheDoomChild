using DChild.Gameplay.Inventories;
using Holysoft.Event;

namespace DChild.Gameplay.Trade
{
    public interface IMerchantStore
    {
        event EventAction<ItemEventArgs> InventoryItemUpdate;
        void SetWares(IInventoryInfo inventory);
        void ResetWares();
    }
}