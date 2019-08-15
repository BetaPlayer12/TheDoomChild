#if UNITY_EDITOR
#endif
using System.Collections.Generic;

namespace DChild.Gameplay.Inventories
{
    public interface IItemContainer
    {
        List<ItemSlot> list { get; }
        void AddItem(ItemData item, int count);
        void SetItem(ItemData item, int count);
        void SetList(ItemContainerData data);
    }
}
