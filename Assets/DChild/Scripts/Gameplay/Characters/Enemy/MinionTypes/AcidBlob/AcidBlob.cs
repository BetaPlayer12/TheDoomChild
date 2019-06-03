﻿using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class AcidBlob : Minion, IFlinch
    {
        [SerializeField]
        private float m_patrolSpeed;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private AttackDamage m_damage;

        private AcidBlobAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_movement.MoveTo(targetPos, m_patrolSpeed);
            m_animation.DoMove();
        }

        public void Move(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_movement.MoveTo(targetPos, m_moveSpeed);
            m_animation.DoMove();
        }

        public void Attack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(AttackRoutine())); 
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, AcidBlobAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator AttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, AcidBlobAnimation.ANIMATION_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
            gameObject.SetActive(false);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, AcidBlobAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
            gameObject.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            m_animation = GetComponent<AcidBlobAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
        }
    }
}
