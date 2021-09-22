using System.Collections;
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
    public class BladeMarrionette3 : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_moveSpeed;
        [SerializeField]
        private float m_hostileMoveSpeedMult;

        [SerializeField]
        private Damage m_damage;

        private BladeMarrionette3Animation m_animation;
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
                m_rootMotion.enabled = true;
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
                m_rootMotion.useY = false;
                m_animation.DoMove();
            }
        }

        public void MovetoTarget(Vector2 target)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = false;
                m_rootMotion.useY = false;
                m_animation.SetAnimation(0, "move", true).TimeScale = m_hostileMoveSpeedMult;
                m_movement.MoveOnGround(target, ((m_moveSpeed * m_hostileMoveSpeedMult) * transform.localScale.x));
            }
        }

        public void MovetoDestination(Vector2 direction)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = false;
                m_rootMotion.useY = false;
                m_animation.SetAnimation(0, "move", true).TimeScale = 1;
                //m_movement.MoveOnGround(transform.right, m_moveSpeed);
                m_movement.MoveTo(direction, m_moveSpeed);
            }
        }

        public void StopMoving()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoIdle1();
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
                StopActiveBehaviour();
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttack();
            }
        }

        public void AttackLong()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttackLong();
            }
        }

        public void AttackTwitch()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttackTwitch();
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "idle")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "idle")
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
            m_animation.DoDeath1();
            yield return new WaitForAnimationComplete(m_animation.animationState, BladeMarrionette3Animation.ANIMATION_DEATH1);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, BladeMarrionette3Animation.ANIMATION_TURN);
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
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, BladeMarrionette3Animation.ANIMATION_DAMAGE);
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
            m_animation = GetComponent<BladeMarrionette3Animation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
