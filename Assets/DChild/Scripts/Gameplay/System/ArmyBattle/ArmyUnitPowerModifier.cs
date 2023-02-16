using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [SerializeField]
    public struct ArmyUnitPowerModifier
    {
        public int rockModifier;
        public int paperModifier;
        public int scissorModifier;

        public ArmyUnitPowerModifier(int rockModifier, int paperModifier, int scissorModifier)
        {
            this.rockModifier = rockModifier;
            this.paperModifier = paperModifier;
            this.scissorModifier = scissorModifier;
        }

        public int GetModifier(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    return rockModifier;
                case UnitType.Paper:
                    return paperModifier;
                case UnitType.Scissors:
                    return scissorModifier;
                default:
                    return 1;
            }
        }
    }
}