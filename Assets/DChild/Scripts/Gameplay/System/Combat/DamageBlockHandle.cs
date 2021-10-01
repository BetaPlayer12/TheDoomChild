namespace DChild.Gameplay.Combat
{
    public struct DamageBlockHandle
    {
        public Damage CalculateBlockedDamage(Damage damage)
        {
            damage.value = 0;
            return damage;
        }
    }
}