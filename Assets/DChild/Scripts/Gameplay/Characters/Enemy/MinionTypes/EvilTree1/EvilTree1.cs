﻿using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EvilTree1 : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_normalMoveSpeed;
        [SerializeField]
        private float m_hostileMoveSpeed;


        [SerializeField]
        private AttackDamage m_damage;

        private EvilTree1Animation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private bool m_isAggro;

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void SetHostility(bool isAggro)
        {
            m_isAggro = isAggro;
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoIdle();
            }
        }

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                //m_animation.DoMove();
                m_animation.SetAnimation(0, "Move", true).TimeScale = !m_isAggro ? m_normalMoveSpeed : m_hostileMoveSpeed;
            }
        }

        public void StopMoving()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoIdle();
                m_movement.Stop();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            StopActiveBehaviour();
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            //TurnCharacter();
        }
        #endregion

        #region "Attack Behaviors"
        public void Attack()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttack();
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Move")
            {
                return m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
            }
            else
            {
                return true;
            }
        }

        public bool IsIdle()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsMoving()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Move")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsTurning()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Turn")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, EvilTree1Animation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, EvilTree1Animation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, EvilTree1Animation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        public void SetRootMotionY(bool condition)
        {
            m_rootMotion.useY = condition;
        }

        protected override void OnDeath()
        {
            //base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<EvilTree1Animation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
