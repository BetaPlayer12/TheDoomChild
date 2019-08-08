﻿using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulEssence;
using Spine;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class Minion : Enemy
    {
        private Vector2 m_initialPosition;

        public override EnemyType enemyType => EnemyType.Minion;
        public override IAttackResistance attackResistance => null;
        public override IStatusResistance statusResistance => null;
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
                //m_colliders.Disable();
                Destroy(this.gameObject);
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
        }

        protected virtual void Start()
        {
            if (animation)
            {
                animation.animationState.Complete += OnAnimationComplete;
            }
        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
