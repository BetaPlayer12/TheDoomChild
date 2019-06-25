using Holysoft;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Refactor.DChild.Gameplay.Characters;

namespace DChild.Gameplay.Environment
{
    public abstract class DestructableObject : Actor, ITarget
    {
        private Hitbox[] m_hitboxes;

        public abstract Vector2 position { get; }
        public abstract IAttackResistance attackResistance { get; }
        public bool isAlive => false;

        public abstract void Heal(int health);
        public abstract void TakeDamage(int totalDamage, AttackType type);

        protected void EnableHitboxes()
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].Enable();
            }
        }

        protected void DisableHitboxes()
        {
            for (int i = 0; i < m_hitboxes.Length; i++)
            {
                m_hitboxes[i].Disable();
            }
        }

        protected virtual void Awake()
        {
            m_hitboxes = GetComponentsInChildren<Hitbox>();
        }


    }
}