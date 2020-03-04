﻿using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Spine;
using Spine.Unity;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Marionette : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_moveSpeed;
        [SerializeField]
        private float m_hostileMoveSpeedMult;
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField]
        private Transform m_projectilePos;
        [SpineEvent, SerializeField]
        private List<string> m_chargeEventName;
        [SerializeField]
        private Collider2D m_legCollider;

        [SerializeField]
        private AttackDamage m_damage;

        private MarionetteAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;
        private IsolatedCharacterPhysics2D m_physics;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private bool m_isDead;

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
                m_rootMotion.enabled = false;
                m_rootMotion.useY = false;
                m_animation.DoIdle1();
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
                m_rootMotion.useY = true;
                m_animation.DoIdle1();
            }
        }

        //public void MovetoTarget(Vector2 target)
        //{
        //    if (m_waitForBehaviourEnd)
        //    {
        //        StopActiveBehaviour();
        //    }
        //    if (Wait())
        //    {
        //        m_rootMotion.enabled = false;
        //        m_rootMotion.useY = false;
        //        m_animation.DoMove();
        //        //var moveDirection = m_facing == Direction.Right ? -transform.right : transform.right;
        //        //m_movement.MoveTowards(moveDirection, m_moveSpeed);
        //        m_movement.MoveTo(target, m_moveSpeed);
        //    }
        //}

        public void MovetoDestination(Vector2 direction, bool isAggro)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = false;
                m_rootMotion.useY = false;
                //m_animation.DoIdleMove();
                if (!isAggro)
                {
                    m_animation.SetAnimation(0, "MARIONETTE3_Idle", true).TimeScale = 1;
                }
                else
                {
                    m_animation.SetAnimation(0, "MARIONETTE3_Idle", true).TimeScale = m_hostileMoveSpeedMult;
                }
                m_movement.MoveTo(direction, !isAggro ? m_moveSpeed : m_moveSpeed * m_hostileMoveSpeedMult);
            }
        }

        public void StopMoving()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                //m_animation.DoIdle1();
                m_animation.SetAnimation(0, "MARIONETTE3_Idle", true).TimeScale = 1;
                m_movement.Stop();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            }
            //TurnCharacter();
        }

        public void Assemble()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_animation.animationState.TimeScale = 1;
            m_physics.simulateGravity = false;
            m_legCollider.enabled = false;
            m_animation.DoAssemble();
            //TurnCharacter();
        }

        public void Standby()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(StandbyRoutine()));
            //TurnCharacter();
        }
        #endregion

        #region "Attack Behaviors"
        public void Attack1()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoAttack1();
            }
        }

        public void Attack2()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoAttack2();
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "MARIONETTE3_Idle")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "MARIONETTE3_Idle")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "MARIONETTE3_Idle")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "MARIONETTE3_Turn")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDead()
        {
            return m_isDead;
        }
        #endregion

        public void ResetAI()
        {
            m_isDead = false;
            m_health.ResetValueToMax();
            EnableHitboxes();
        }

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            int rand = Random.Range(1, 3);
            if(rand == 1)
            {
                m_animation.DoDeath2();
            }
            else
            {
                m_animation.DoDeath3();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, rand == 1 ? MarionetteAnimation.ANIMATION_MARIONETTE3_DEATH2 : MarionetteAnimation.ANIMATION_MARIONETTE3_DEATH3);
            m_physics.simulateGravity = true;
            m_legCollider.enabled = true;
            yield return null;
            m_animation.animationState.TimeScale = 0;
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, MarionetteAnimation.ANIMATION_MARIONETTE3_TURN);
            m_animation.DoIdle1();
            yield return null;
            TurnCharacter();
            StopActiveBehaviour();
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoDamage();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, MarionetteAnimation.ANIMATION_MARIONETTE3_FLINCH);
            StopActiveBehaviour();
        }

        private IEnumerator StandbyRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_movement.Stop();
            int rand = Random.Range(1, 3);
            if (rand == 1)
            {
                m_animation.DoDeath2();
            }
            else
            {
                m_animation.DoDeath3();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, rand == 1 ? MarionetteAnimation.ANIMATION_MARIONETTE3_DEATH2 : MarionetteAnimation.ANIMATION_MARIONETTE3_DEATH3);
            m_physics.simulateGravity = true;
            m_legCollider.enabled = true;
            yield return null;
            m_animation.animationState.TimeScale = 0;
            StopActiveBehaviour();
        }

        public void SetRootMotionY(bool condition)
        {
            m_rootMotion.useY = condition;
        }

        protected override void OnDeath()
        {
            //base.OnDeath();
            m_isDead = true;
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_chargeEventName[0])
            {
                //Debug.Log("GAGOO NU GINAGAWA MO");
                //GameObject shoot = Instantiate(m_projectile, m_projectilePos.position, Quaternion.Euler(new Vector3(0, 0, transform.localScale.x == 1 ? 180 : 0)));
            }
        }

        protected override void Start()
        {
            base.Start();
            m_animation.DoAssemble();
            m_animation.animationState.TimeScale = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_physics = GetComponent<IsolatedCharacterPhysics2D>();
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<MarionetteAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }

            //Spine Event Listener
            var skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            if (skeletonAnimation == null) return;

            skeletonAnimation.AnimationState.Event += HandleEvent;
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
