using DChild.Gameplay.Combat;

namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyCombatInfo
    {
        ArmyAttack attackInfo { get; }
        ArmyUnitModifier damageReductionModifier { get; }
        Health troopCount { get; }
        bool canAttack { get; }
    }
}