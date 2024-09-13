using DChild.Gameplay.Combat;

namespace DChild.Gameplay.ArmyBattle
{
    public interface IArmyCombatInfo
    {
        ArmyAttack attackInfo { get; }
        ArmyDamageTypeModifier damageReductionModifier { get; }
        Health troopCount { get; }
        bool canAttack { get; }
    }
}