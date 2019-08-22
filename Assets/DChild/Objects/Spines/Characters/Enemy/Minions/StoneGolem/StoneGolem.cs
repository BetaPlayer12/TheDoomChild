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
    public class StoneGolem : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_hostileMoveSpeedMult;
        [SerializeField]
        private float m_chargeSpeed;
        [SerializeField]
        private float m_chargeTime;

        [SerializeField]
        private AttackDamage m_damage;

        private StoneGolemAnimation m_animation;
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

        public void SetHostility(bool isHostile)
        {
            if (isHostile && m_animation.animationState.TimeScale < 1)
            {
                m_animation.animationState.TimeScale = m_animation.animationState.TimeScale + (0.25f * Time.deltaTime);
            }
            else if (!isHostile && m_animation.animationState.TimeScale > 0)
            {
                m_animation.animationState.TimeScale = m_animation.animationState.TimeScale - (0.25f * Time.deltaTime);
            }
            else
            {
                m_animation.animationState.TimeScale = !isHostile ? 0 : 1;
            }
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

        public void MovetoDestination(bool isAggro)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.SetAnimation(0, "walk", true).TimeScale = !isAggro ? 1 : m_hostileMoveSpeedMult;
                //m_movement.MoveTo(direction, m_moveSpeed);
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

        public void AttackCharge(Vector2 direction)
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(ChargeRoutine(direction)));
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "walk")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "walk")
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

        public bool IsNormal()
        {
            return m_animation.animationState.TimeScale == 1 ? true : false;
        }
        #endregion

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoDeath1();
            yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_DEATH1);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_TURN);
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
            yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        private IEnumerator ChargeRoutine(Vector2 direction)
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_movement.Stop();
            //No Root Motion
            //m_animation.DoChargePreparation();
            //yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_CHARGE_PREPARATION);
            //m_movement.MoveTo(direction, m_chargeSpeed);
            //m_animation.DoChargeLoop();
            //yield return new WaitForSeconds(m_chargeTime);
            //m_animation.DoChargeEnd();
            //yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_CHARGE_END);
            //Root Motion
            m_animation.DoChargeTest();
            yield return new WaitForAnimationComplete(m_animation.animationState, StoneGolemAnimation.ANIMATION_CHARGE_TEST);
            m_animation.DoIdle1();
            yield return null;
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

        protected override void Start()
        {
            base.Start();
            m_animation.animationState.TimeScale = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<StoneGolemAnimation>();
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
