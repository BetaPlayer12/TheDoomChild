namespace DChild.Gameplay.ArmyBattle
{
    public struct ArmyDamage
    {
        public DamageType type;
        public int value;

        public ArmyDamage(DamageType type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }
}