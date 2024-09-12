using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [SerializeField]
    public struct ArmyUnitModifier
    {
        [SerializeField]
        private float m_rockModifier;
        [SerializeField]
        private float m_paperModifier;
        [SerializeField]
        private float m_scissorModifier;

        public ArmyUnitModifier(float rockModifier, float paperModifier, float scissorModifier)
        {
            this.m_rockModifier = rockModifier;
            this.m_paperModifier = paperModifier;
            this.m_scissorModifier = scissorModifier;
        }

        public float GetModifier(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    return m_rockModifier;
                case UnitType.Paper:
                    return m_paperModifier;
                case UnitType.Scissors:
                    return m_scissorModifier;
                default:
                    return 1;
            }
        }

        public void SetModifier(UnitType unitType, float value)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    m_rockModifier = value;
                    break;
                case UnitType.Paper:
                    m_paperModifier = value;
                    break;
                case UnitType.Scissors:
                    m_paperModifier = value;
                    break;
            }
        }

        public void AddModifier(UnitType unitType, float value)
        {
            switch (unitType)
            {
                case UnitType.Rock:
                    m_rockModifier += value;
                    break;
                case UnitType.Paper:
                    m_paperModifier += value;
                    break;
                case UnitType.Scissors:
                    m_paperModifier += value;
                    break;
            }
        }

        public void ResetModifiers()
        {
            m_rockModifier = 1f;
            m_paperModifier = 1f;
            m_paperModifier = 1f;
        }
    }
}