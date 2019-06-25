using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.AI
{
    public class AITargetInfo
    {
        public bool isCharacter { get; private set; }
        public bool isValid { get; private set; }
        private IDamageable m_damageable;
        private Character m_target;

        public AITargetInfo()
        {
            m_damageable = null;
            this.m_target = null;
            isCharacter = false;
            isValid = false;
        }

        public AITargetInfo(IDamageable damageable, Character target)
        {
            m_damageable = damageable;
            this.m_target = target;
            isCharacter = target;
            isValid = damageable != null;
        }

        public AITargetInfo(IDamageable damageable)
        {
            m_damageable = damageable;
            isCharacter = false;
            isValid = damageable != null;
        }

        public HorizontalDirection facing => m_target.facing;
        public Vector2 position => m_damageable.position;

        public void Set(IDamageable damageable, Character target)
        {
            m_damageable = damageable;
            this.m_target = target;
            isCharacter = target;
            isValid = damageable != null;
        }

        public void Set(IDamageable damageable)
        {
            m_damageable = damageable;
            isCharacter = false;
            isValid = damageable != null;
        }
    }
}