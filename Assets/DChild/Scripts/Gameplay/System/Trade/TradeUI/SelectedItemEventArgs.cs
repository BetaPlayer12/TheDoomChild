using DChild.Gameplay.Inventories;
using Holysoft.Event;

namespace DChild.Gameplay.Trade.UI
{
    public class SelectedItemEventArgs : IEventActionArgs
    {
        private ITradeItem m_item;

        public ITradeItem item => m_item;

        public void Set(ITradeItem storedItem) => m_item = storedItem;
    }

}