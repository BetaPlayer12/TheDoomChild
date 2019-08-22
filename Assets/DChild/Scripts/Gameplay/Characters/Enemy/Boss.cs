﻿using DChild.Gameplay.Combat;
using Spine;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class Boss : Enemy
    {
        private Vector2 m_initialPosition;
        protected AttackDamage[] m_currentDamage;

        public override EnemyType enemyType => EnemyType.Boss;
        public override IAttackResistance attackResistance => null;
        protected abstract AttackDamage startDamage { get; }

        public override void InitializeAs(bool isAlive)
        {
            if (isAlive)
            {
                m_health.ResetValueToMax();
                EnableHitboxes();
                m_colliders.Enable();
                transform.position = m_initialPosition;
                m_brain.ResetBrain();
                m_behaviour.SetActiveBehaviour(null);
                gameObject.SetActive(true);
                animation?.DoIdle();
                ResetValues();
            }
            else
            {
                m_health.Empty();
                DisableHitboxes();
                m_colliders.Disable();
                gameObject.SetActive(false);
            }
        }

        protected virtual void OnAnimationComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == CombatCharacterAnimation.ANIMATION_DEATH)
            {
                m_colliders.Disable();
            }
        }

        protected override void OnDeath()
        {
            animation?.DoDeath();
        }

        protected virtual void ResetValues()
        {

        }

        protected override void Awake()
        {
            base.Awake();
            m_initialPosition = transform.position;
            m_currentDamage = new AttackDamage[1];
        }

        protected virtual void Start()
        {
            m_currentDamage[0] = startDamage;
            if (animation)
            {
                animation.animationState.Complete += OnAnimationComplete;
            }
        }
    }
}
