using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    public class ArmyAbilityData : SerializedScriptableObject
    {
        [SerializeField]
        private ArmyCategoryCompositionInfo m_requirement;
        [SerializeField]
        private IArmyAbilityEffect[] m_effects;

        public ArmyCategoryCompositionInfo requirement => m_requirement;

        public void ApplyEffect(Army owner, Army opponent)
        {
            for (int i = 0; i < m_effects.Length; i++)
            {
                m_effects[i].ApplyEffect(owner, opponent);
            }
        }
    }
}