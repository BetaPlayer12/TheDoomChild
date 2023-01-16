namespace DChild.Gameplay.ArmyBattle
{
    public struct ArmyAttack
    {
        public UnitType type;
        public int value;

        public ArmyAttack(UnitType type, int value)
        {
            this.type = type;
            this.value = value;
        }
    }
}