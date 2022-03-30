using DChild.Gameplay.Inventories;

namespace DChild.Menu.Trading
{
    public interface IMerchantStore
    {
        public void SetWaresReference(InventoryData inventory);
        public void ResetWares();
    }
}