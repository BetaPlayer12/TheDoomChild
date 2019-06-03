namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public class BasicAttackResistance : AttackResistance
    {
        public override float GetResistance(AttackType type) => m_resistanceInfo.ContainsKey(type) ? m_resistanceInfo[type] : 0;
    }
}