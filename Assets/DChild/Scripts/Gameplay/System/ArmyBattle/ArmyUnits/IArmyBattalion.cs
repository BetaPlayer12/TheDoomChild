using DChild.Gameplay.ArmyBattle.Units;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Battalion
{
    public interface IArmyBattalion
    {
        Vector2 centerPosition { get;}
        ArmyUnitsHandle GetUnitHandle(DamageType type);
    }
}