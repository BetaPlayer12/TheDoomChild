using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "BonusConversion", menuName = "DChild/Player/Bonus Conversion")]
    public class BonusConversion : ScriptableObject
    {
        [System.Serializable]
        private struct BonusHandle
        {
            [SerializeField]
            private Attribute m_fromAttribute;
            [SerializeField, HideLabel]
            private RatioCalulation m_calcuation;

            public Attribute fromAttribute => m_fromAttribute;
            public int CalculateOutput(int value) => m_calcuation.CalculateOutput(value);
        }

        [SerializeField]
        private BonusHandle[] m_bonuses;

        public int GetBonus(IAttributes attributes)
        {
            int bonus = 0;
            for (int i = 0; i < m_bonuses.Length; i++)
            {
                var handle = m_bonuses[i];
                var attributeValue = attributes.GetValue(handle.fromAttribute);
                bonus += handle.CalculateOutput(attributeValue);
            }
            return bonus;
        }
    }
}