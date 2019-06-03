using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Attributes
{
    [System.Serializable]
    public class Strength : Attribute, IStrength
    {
        private int m_toAttack;
        private int m_bonusAttack;

        public int toAttack => m_toAttack;
        public int bonusAttack => m_bonusAttack;

        public override void CalculateBonuses()
        {
            m_toAttack = m_value * 3;
            m_bonusAttack = Mathf.FloorToInt(m_value / 5);
        }
    }
}