using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct SystemModifer : ISoulSkillModule
    {
        private enum Field
        {
            SoulEssence_Drop
        }

        [SerializeField]
        private Field m_systemField;
        [SerializeField]
        private float m_value;

        public void AttachTo(IPlayer player)
        {
            switch (m_systemField)
            {
                case Field.SoulEssence_Drop:
            GameplaySystem.modifiers.minionSoulEssenceDrop = m_value;
                    break;
            }
        }

        public void DetachFrom(IPlayer player)
        {
            switch (m_systemField)
            {
                case Field.SoulEssence_Drop:
                    GameplaySystem.modifiers.minionSoulEssenceDrop = 1;
                    break;
            }
        }
    }
}