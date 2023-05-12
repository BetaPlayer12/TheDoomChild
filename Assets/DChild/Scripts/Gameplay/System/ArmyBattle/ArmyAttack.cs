using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public struct ArmyAttack
    {
        private UnitType m_type;
        private int m_baseValue;
        private float m_modifier;
        private int m_value;

        public UnitType type => m_type;
        public int baseValue => m_baseValue;
        public float modifier => m_modifier;
        public int value => m_value;

        public bool isModified => modifier != 1;

        public ArmyAttack(UnitType type, int value, float modifier)
        {
            m_type = type;
            m_baseValue = value;
            m_modifier = modifier;
            m_value = ArmyBattleUtlity.CalculateModifiedPower(value,modifier);
        }
    }
}