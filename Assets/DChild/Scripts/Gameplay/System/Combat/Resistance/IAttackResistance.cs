namespace DChild.Gameplay.Combat
{
    public interface IAttackResistance
    {
        float GetResistance(AttackType type);
        void SetResistance(AttackType type, AttackResistanceType resistanceType);
    }
}