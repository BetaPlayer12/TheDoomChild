using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusInfliction;

namespace DChild.Gameplay.Characters.Players.Equipments
{
    public interface IWeapon : IAttackStats, IValueChange
    {
        void AddDamage(AttackType type, int value);
        void ReduceDamage(AttackType type, int value);

        bool canInflictStatusEffects { get; }
        StatusInflictionInfo[] statusToInflict { get; }
        void AddStatusInfliction(StatusEffectType type, float chance);
        void ReduceStatusInfliction(StatusEffectType type, float chance);
    }
}