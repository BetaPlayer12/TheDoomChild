using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Attributes
{
    [System.Serializable]
    public class Intelligence : Attribute, IIntelligence
    {
        private int m_toMagic;
        private int m_bonusMagic;
        private int m_bonusMagicAttack;
        private int m_bonusMagicDefense;

        public int toMagic => m_toMagic;
        public int bonusMagic => m_bonusMagic;
        public int bonusMagicAttack => m_bonusMagicAttack;
        public int bonusMagicDefense => m_bonusMagicDefense;

        public override void CalculateBonuses()
        {
            m_toMagic = m_value * 5;
            m_bonusMagic = Mathf.FloorToInt(m_value / 10);
            m_bonusMagicAttack = Mathf.FloorToInt(m_value / 12);
            m_bonusMagicDefense = Mathf.FloorToInt(m_value / 10);
        }
    }
}