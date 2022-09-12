using DChild.Gameplay.Items;
using Holysoft.Event;
namespace DChild.Gameplay.Inventories
{
    public class ItemEventArgs : IEventActionArgs
    {
        public ItemData data { get; private set; }
        public int currentCount { get; private set; }
        public int countModification { get; private set; }

        public void Initialize(ItemData data, int currentCount, int countModification)
        {
            this.data = data;
            this.currentCount = currentCount;
            this.countModification = countModification;
        }
    }
}
