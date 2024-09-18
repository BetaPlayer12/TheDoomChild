using System;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public struct ArmyDamageTypeModifier
    {
        [SerializeField]
        private float m_meleeModifier;
        [SerializeField]
        private float m_rangeModifier;
        [SerializeField]
        private float m_magicModifier;

        public ArmyDamageTypeModifier(float meleeModifier, float rangeModifier, float magicModifier)
        {
            m_meleeModifier = meleeModifier;
            m_rangeModifier = rangeModifier;
            m_magicModifier = magicModifier;
        }

        public float GetModifier(DamageType type)
        {
            switch (type)
            {
                case DamageType.Melee:
                    return m_meleeModifier;
                case DamageType.Range:
                    return m_rangeModifier;
                case DamageType.Magic:
                    return m_magicModifier;
                default:
                    return 1;
            }
        }
        public void SetModifier(DamageType type, float value)
        {
            switch (type)
            {
                case DamageType.Melee:
                    m_meleeModifier = value;
                    break;
                case DamageType.Range:
                    m_rangeModifier = value;
                    break;
                case DamageType.Magic:
                    m_magicModifier = value;
                    break;
                default:
                    break;
            }
        }
        public void AddModifier(DamageType type, float value)
        {
            switch (type)
            {
                case DamageType.Melee:
                    m_meleeModifier += value;
                    break;
                case DamageType.Range:
                    m_rangeModifier += value;
                    break;
                case DamageType.Magic:
                    m_magicModifier += value;
                    break;
                default:
                    break;
            }
        }
        public void ResetModifiers()
        {
            m_meleeModifier = 1f;
            m_rangeModifier = 1f;
            m_magicModifier = 1f;
        }
    }
}