using System;
using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonWarrior : Minion, IFlinch
    {
        [SerializeField]
        [LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;

        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;

        private ISensorFaceRotation[] m_sensorRotator;

        private PhysicsMovementHandler2D m_movement;
        private SkeletonWarriorAnimation m_animation;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override AttackDamage startDamage => m_damage;

        public void Slash()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SlashAttackRoutine()));
        }

        public void Stab()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(StabAttackRoutine()));
        }

        public void Patrol(Vector2 position)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.MoveOnGround(position, m_patrolSpeed);
            m_animation.DoPatrol();
        }

        public void MoveTo(Vector2 position)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.MoveOnGround(position, m_moveSpeed);
            m_animation.DoMove();
        }

        public void PlayerDetect()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(DetectRoutine()));
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

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
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
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            RotateSensor();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDetect();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_DETECT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SlashAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSlash();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_SLASH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator StabAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoStab();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_STAB);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonWarriorAnimation.ANIMATION_DEATH);
            gameObject.SetActive(false);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);      
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_animation = GetComponent<SkeletonWarriorAnimation>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();          
        }

        protected override void Start()
        {
            base.Start();
            RotateSensor();
        }
    }
}