using DChild.Gameplay.Items;

namespace DChild.Menu.Trade
{
    public interface ITraderAskingPrice
    {
        int GetAskingPrice(ItemData data, TradeType tradeType);
    }
}