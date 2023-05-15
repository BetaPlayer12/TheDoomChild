using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public static class ArmyBattleUtlity
    {
        public static int CalculateModifiedPower(int baseValue, float modifier) => Mathf.FloorToInt(baseValue * modifier);
    }
}