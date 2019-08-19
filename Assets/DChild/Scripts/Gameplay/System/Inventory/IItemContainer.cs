#if UNITY_EDITOR
#endif
using System.Collections.Generic;

namespace DChild.Gameplay.Inventories
{
    public interface IItemContainer
    {
        bool restrictSize { get; }
        int MaxSize { get; }
        int Count { get; }
        ItemSlot GetSlot(int index);
        void AddItem(ItemData item, int count);
        void SetItem(ItemData item, int count);
        void SetList(ItemContainerData data);
    }
}
