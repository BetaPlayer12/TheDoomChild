using System.Collections;
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
    public class HorseSea : Minion, IFlinch, ITerrainPatroller
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
        private Damage m_damage;

        private HorseSeaAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private bool m_isHostile;

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void SetHostility(bool isHostile)
        {
            m_isHostile = isHostile;
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
                m_animation.DoIdleMove();
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
                m_animation.DoIdleMove();
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
                    m_animation.SetAnimation(0, "Idle/Move", true).TimeScale = 1;
                }
                else
                {
                    m_animation.SetAnimation(0, "Idle/Move", true).TimeScale = m_hostileMoveSpeedMult;
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
                m_animation.SetAnimation(0, "Idle/Move", true).TimeScale = 1;
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
            if (m_animation.GetCurrentAnimation(0).ToString() != "Idle/Move" && m_animation.GetCurrentAnimation(0).ToString() != "Idle/Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle/Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle/Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Turn" || m_animation.GetCurrentAnimation(0).ToString() == "Turn2")
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
            yield return new WaitForAnimationComplete(m_animation.animationState, HorseSeaAnimation.ANIMATION_DEATH);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            if (!m_isHostile)
            {
                m_animation.DoTurn2();
            }
            else
            {
                m_animation.DoTurn();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState,!m_isHostile ? HorseSeaAnimation.ANIMATION_TURN2 : HorseSeaAnimation.ANIMATION_TURN);
            m_animation.DoIdleMove();
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
            yield return new WaitForAnimationComplete(m_animation.animationState, HorseSeaAnimation.ANIMATION_FLINCH);
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

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_chargeEventName[0])
            {
                //Debug.Log("GAGOO NU GINAGAWA MO");
                GameObject shoot = Instantiate(m_projectile, m_projectilePos.position, Quaternion.Euler(new Vector3(0, 0, transform.localScale.x == 1 ? 180 : 0)));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<HorseSeaAnimation>();
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

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
