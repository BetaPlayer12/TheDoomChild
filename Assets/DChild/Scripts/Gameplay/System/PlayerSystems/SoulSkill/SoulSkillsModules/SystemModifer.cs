using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct SystemModifer : ISoulSkillModule
    {
        private enum Field
        {
            SoulEssence_Drop,
            SoulEssence_Absorbption
        }

        [SerializeField]
        private Field m_systemField;
        [SerializeField]
        private float m_value;

        public void AttachTo(int soulSkillInstanceID, IPlayer player)
        {
            switch (m_systemField)
            {
                case Field.SoulEssence_Drop:
                    GameplaySystem.modifiers.minionSoulEssenceDrop = m_value;
                    break;
                case Field.SoulEssence_Absorbption:
                    GameplaySystem.modifiers.SoulessenceAbsorption = m_value;
                    break;
            }
        }

        public void DetachFrom(int soulSkillInstanceID, IPlayer player)
        {
            switch (m_systemField)
            {
                case Field.SoulEssence_Drop:
                    GameplaySystem.modifiers.minionSoulEssenceDrop = 1;
                    break;
                case Field.SoulEssence_Absorbption:
                    GameplaySystem.modifiers.SoulessenceAbsorption = 1;
                    break;
            }
        }
    }
}