using DChild.Gameplay.Items;

namespace DChild.Menu.Trading
{
    public interface ITraderAskingPrice
    {
        int GetAskingPrice(ItemData data, TradeType tradeType);
    }
}