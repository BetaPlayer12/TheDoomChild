using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Attributes
{
    [System.Serializable]
    public class Vitality : Attribute, IVitality
    {
        private int m_toHealth;
        private int m_bonusHealth;
        private int m_bonusDefense;
        private int m_bonusMagicDefense;

        public int toHealth => m_toHealth;
        public int bonusHealth => m_bonusHealth;
        public int bonusDefense => m_bonusDefense;
        public int bonusMagicDefense => m_bonusMagicDefense;


        public override void CalculateBonuses()
        {
            m_toHealth = m_value * 15;
            m_bonusHealth = Mathf.FloorToInt(m_value / 6);
            m_bonusDefense = Mathf.FloorToInt(m_value / 5);
            m_bonusMagicDefense = Mathf.FloorToInt(m_value / 15);
        }
    }
}