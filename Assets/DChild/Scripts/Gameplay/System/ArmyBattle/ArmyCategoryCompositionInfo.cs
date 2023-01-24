using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public struct ArmyCategoryCompositionInfo
    {
        [SerializeField]
        private ArmyCharacterCategory m_characterCategory;
        [SerializeField, MinValue(1)]
        public int characterCount;

        public ArmyCategoryCompositionInfo(ArmyCharacterCategory characterCategory, int characterCount = 1)
        {
            m_characterCategory = characterCategory;
            this.characterCount = characterCount;
        }
    }
}