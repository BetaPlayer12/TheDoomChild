namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public class BasicAttackResistance : AttackResistance
    {
        public override float GetResistance(AttackType type) => m_resistance.ContainsKey(type) ? m_resistance[type] : 0;
    }
}