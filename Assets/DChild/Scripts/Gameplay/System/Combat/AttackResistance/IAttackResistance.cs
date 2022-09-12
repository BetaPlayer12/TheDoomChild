namespace DChild.Gameplay.Combat
{
    public interface IAttackResistance
    {
        float GetResistance(DamageType type);
        //void SetResistance(AttackType type, AttackResistanceType resistanceType);
    }
}