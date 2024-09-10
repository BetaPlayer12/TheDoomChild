namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public struct ArmyTurnAction
    {
        public int troopCount;
        public ArmyDamage attack;

        public float damageModifier;
        public float damageResistanceModifier;
    }
}