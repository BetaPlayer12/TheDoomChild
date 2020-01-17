using DChild.Gameplay.Items;
using Holysoft.Event;
namespace DChild.Gameplay.Inventories
{
    public class ItemEventArgs : IEventActionArgs
    {
        public ItemData data { get; private set; }
        public int count { get; private set; }

        public void Initialize(ItemData data, int count)
        {
            this.data = data;
            this.count = count;
        }
    }
}
