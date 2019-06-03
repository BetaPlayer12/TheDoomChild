using DChild.Gameplay.Combat.StatusInfliction;

namespace DChild.Gameplay.Combat
{
    public interface IStatusResistance
    {
        StatusResistanceType GetResistance(StatusEffectType type);
        void SetResistance(StatusEffectType type, StatusResistanceType resistanceType);
    }
}