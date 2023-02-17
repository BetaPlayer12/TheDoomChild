using System;

namespace DChild.Gameplay.ArmyBattle
{
    [Flags]
    public enum ArmyCharacterCategory
    {
        Fox  = 1 << 0,
        Guard = 1 << 1,
        Pirate = 1 << 2,
    }
}