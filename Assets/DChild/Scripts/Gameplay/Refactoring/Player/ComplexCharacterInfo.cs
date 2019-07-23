using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class ComplexCharacterInfo
    {
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private CharacterPhysics2D m_physics;
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private AnimationParametersData m_animationParametersData;

        public Character character => m_character;

        public CharacterState state => m_state;
        public Animator animator => m_animator;

        public AnimationParametersData animationParametersData => m_animationParametersData;

        public CharacterPhysics2D physics => m_physics;
    }
}