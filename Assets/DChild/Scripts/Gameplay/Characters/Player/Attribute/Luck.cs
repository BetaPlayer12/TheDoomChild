using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Attributes
{
    [System.Serializable]
    public class Luck : Attribute, ILuck
    {
        private float m_critChance;
        private float m_statusChance;

        public float critChance => m_critChance;
        public float statusChance => m_statusChance;

        public override void CalculateBonuses()
        {
            m_critChance = Mathf.CeilToInt(m_value * 0.3f) / 100f;
            m_statusChance = Mathf.CeilToInt(m_value / 5) / 100f;
        }
    }
}