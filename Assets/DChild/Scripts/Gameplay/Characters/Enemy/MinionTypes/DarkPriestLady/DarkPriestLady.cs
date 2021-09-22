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
    public class DarkPriestLady : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_moveSpeed;
        [SerializeField]
        private GameObject m_hitBoxes;
        [SerializeField]
        private GameObject m_explosionGo;
        [SerializeField]
        private Transform m_explosionTF;

        [SerializeField]
        private Damage m_damage;

        private DarkPriestLadyAnimation m_animation;
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
                if (!m_isHostile)
                {
                    m_animation.DoIdleMove();
                }
                else
                {
                    m_animation.DoIdle2Move();
                }
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
                m_rootMotion.enabled = false;
                m_rootMotion.useY = false;
                if (!m_isHostile)
                {
                    m_animation.DoIdleMove();
                }
                else
                {
                    m_animation.DoIdle2Move();
                }
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
                if (!m_isHostile)
                {
                    m_animation.DoIdleMove();
                }
                else
                {
                    m_animation.DoIdle2Move();
                }
                //var moveDirection = m_facing == Direction.Right ? -transform.right : transform.right;
                //m_movement.MoveTowards(moveDirection, m_moveSpeed);
                m_movement.MoveTo(target, m_moveSpeed);
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
                if (!m_isHostile)
                {
                    m_animation.DoIdleMove();
                }
                else
                {
                    m_animation.DoIdle2Move();
                }
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
                m_animation.DoIdleMove();
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

        public void Detect()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoDetect(m_isHostile);
        }

        public void Stun()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoStun();
        }
        #endregion

        #region "Attack Behaviors"
        public void Attack1()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(ExplodeRoutine()));
            }
        }

        public void AttackEnraged()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoAttackEnraged(m_isHostile);
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Idle/Move" && m_animation.GetCurrentAnimation(0).ToString() != "Idle2/Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle/Move" || m_animation.GetCurrentAnimation(0).ToString() == "Idle2/Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle/Move" || m_animation.GetCurrentAnimation(0).ToString() == "Idle2/Move")
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
            yield return new WaitForAnimationComplete(m_animation.animationState, DarkPriestLadyAnimation.ANIMATION_DEATH);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, DarkPriestLadyAnimation.ANIMATION_TURN);
            if (!m_isHostile)
            {
                m_animation.DoIdleMove();
            }
            else
            {
                m_animation.DoIdle2Move();
            }
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
            yield return new WaitForAnimationComplete(m_animation.animationState, m_isHostile != true ? DarkPriestLadyAnimation.ANIMATION_FLINCH1 : DarkPriestLadyAnimation.ANIMATION_FLINCH2);
            StopActiveBehaviour();
        }

        private IEnumerator ExplodeRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_animation.DoAttack1(m_isHostile);
            yield return new WaitForSeconds(1.3f);
            m_hitBoxes.SetActive(false);
            GameObject explosion = Instantiate(m_explosionGo, m_explosionTF.position, Quaternion.identity);
            //Debug.Log("Boom Explode");
            yield return new WaitForAnimationComplete(m_animation.animationState, DarkPriestLadyAnimation.ANIMATION_ATTACK1);
            Destroy(this.gameObject);
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
            m_animation = GetComponent<DarkPriestLadyAnimation>();
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
