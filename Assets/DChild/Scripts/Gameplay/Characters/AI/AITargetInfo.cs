using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class AITargetInfo
    {
        public bool isCharacter { get; private set; }
        public bool isValid { get; private set; }
        public bool doesTargetExist => m_damageable != null;
        private IDamageable m_damageable;
        private Character m_target;
        private IGroundednessState m_characterGroundedState;

        public AITargetInfo()
        {
            m_damageable = null;
            this.m_target = null;
            isCharacter = false;
            isValid = false;
            m_characterGroundedState = null;
        }

        public AITargetInfo(IDamageable damageable, Character target)
        {
            m_damageable = damageable;
            this.m_target = target;
            isCharacter = target;
            isValid = damageable != null;
            m_characterGroundedState = target.GetComponent<IGroundednessState>();
        }

        public IDamageable GetTargetDamagable()
        {
            return m_damageable;
        }

        public AITargetInfo(IDamageable damageable)
        {
            m_damageable = damageable;
            isCharacter = false;
            isValid = damageable != null;
        }

        public HorizontalDirection facing => m_target.facing;
        public Vector2 position => m_damageable.position;
        //For parenting purposes of AI to the target.. some behaviours need this
        public Transform transform => m_damageable.transform;

        public bool isCharacterGrounded => m_characterGroundedState.isGrounded;

        public void Set(IDamageable damageable, Character target)
        {
            m_damageable = damageable;
            this.m_target = target;
            isCharacter = target;
            isValid = damageable != null;
            if(target != null)
                m_characterGroundedState = target.GetComponentInChildren<IGroundednessState>();

        }

        public void Set(IDamageable damageable)
        {
            m_damageable = damageable;
            isCharacter = false;
            isValid = damageable != null;
        }
    }
}