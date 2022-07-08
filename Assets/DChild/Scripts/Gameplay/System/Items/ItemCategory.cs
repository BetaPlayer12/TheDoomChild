using System;

namespace DChild.Gameplay.Items
{
    [Flags]
    public enum ItemCategory
    {
        Throwable = 1 << 0,
        Consumable = 1 << 1,
        Quest = 1 << 2,
        Key = 1 << 3,
        SoulSkill = 1 << 4,

        QuickItem = Throwable | Consumable,
        All = Throwable | Consumable | Quest| Key| SoulSkill,
    }
}
