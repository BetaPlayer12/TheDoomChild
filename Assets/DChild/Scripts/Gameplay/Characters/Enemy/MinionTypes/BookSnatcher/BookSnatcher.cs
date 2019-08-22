﻿using System.Collections;
using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BookSnatcher : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;

        [SerializeField]
        private AttackDamage m_damage;

        private BookSnatcherAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
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

        public void Walk()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoWalk();
            }
        }

        public void Run()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoRun();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            //TurnCharacter();
        }

        public void BookOpen()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoBookOpen();
            }
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
            if (m_animation.GetCurrentAnimation(0).ToString() != "Walk" && m_animation.GetCurrentAnimation(0).ToString() != "Run")
            {
                //Debug.Log("Must Wait");
                return m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
            }
            else
            {
                //Debug.Log("Don't Wait");
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Walk" || m_animation.GetCurrentAnimation(0).ToString() == "Run")
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
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            //m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, BookSnatcherAnimation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, BookSnatcherAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            //m_movement.Stop();
            m_animation.DoDamageAnim();
            yield return new WaitForAnimationComplete(m_animation.animationState, BookSnatcherAnimation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        public void SetRootMotionY(bool condition)
        {
            m_rootMotion.useY = condition;
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<BookSnatcherAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }
        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}