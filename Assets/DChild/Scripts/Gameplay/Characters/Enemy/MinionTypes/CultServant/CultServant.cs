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
    public class CultServant : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private Damage m_damage;

        private CultServantAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void Idle(bool isHostile)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                if (!isHostile)
                {
                    //Debug.Log("Idle without book");
                    m_animation.DoIdleWithoutBook();
                }
                else
                {
                    //Debug.Log("Idle with book");
                    m_animation.DoIdleWithBook();
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
                m_animation.DoMoveWithoutBook();
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
                m_animation.DoMoveWithBook();
                m_movement.MoveOnGround(target, m_moveSpeed * (transform.localScale.x));
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
                m_animation.DoMoveWithoutBook();
                //m_movement.MoveOnGround(transform.right, m_moveSpeed);
                m_movement.MoveTo(direction, m_moveSpeed);
            }
        }

        public void StopMoving(bool isHostile)
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                if (!isHostile)
                {
                    //Debug.Log("Idle without book");
                    m_animation.DoIdleWithoutBook();
                }
                else
                {
                    //Debug.Log("Idle with book");
                    m_animation.DoIdleWithBook();
                }
                m_movement.Stop();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine(m_animation.GetCurrentAnimation(0).ToString() == "Idle_With_Book" ? true : false)));
            //TurnCharacter();
        }

        public void TurnWithBook()
        {
            //StopActiveBehaviour();
            //m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoTurnWithBook();
        }

        public void TurnWithoutBook()
        {
            //StopActiveBehaviour();
            //m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoTurnWithoutBook();
        }

        public void Detect()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoDetect();
        }

        public void Teleport()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoTeleport();
        }

        public void TeleportAway()
        {
            m_rootMotion.enabled = false;
            m_rootMotion.useY = false;
            m_animation.DoTeleportAway();
        }
        #endregion

        #region "Attack Behaviors"
        public void AttackCastingSpell()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttackCastingSpell();
            }
        }

        public void AttackConjureBooks()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttackConjureBooks();
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Move_With_Book" && m_animation.GetCurrentAnimation(0).ToString() != "Move_Without_Book")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle_With_Book" || m_animation.GetCurrentAnimation(0).ToString() == "Idle_Without_Book")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Move_With_Book" || m_animation.GetCurrentAnimation(0).ToString() == "Move_Without_Book")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Turn_With_Book" && m_animation.GetCurrentAnimation(0).ToString() == "Turn_WithoutBook")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasTeleportedAway()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Teleport_Away")
            {
                if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).AnimationTime > 0.75f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
            yield return new WaitForAnimationComplete(m_animation.animationState, CultServantAnimation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine(bool isHostile)
        {
            m_waitForBehaviourEnd = true;
            if (!isHostile)
            {
                m_animation.DoTurnWithoutBook();
            }
            else
            {
                m_animation.DoTurnWithBook();
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, !isHostile ? CultServantAnimation.ANIMATION_TURN_WITHOUT_BOOK : CultServantAnimation.ANIMATION_TURN_WITH_BOOK);
            if (!isHostile)
            {
                m_animation.DoIdleWithoutBook();
            }
            else
            {
                m_animation.DoIdleWithBook();
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
            m_animation.DoFlinchBookMove();
            yield return new WaitForAnimationComplete(m_animation.animationState, CultServantAnimation.ANIMATION_FLINCH_BOOK_MOVE);
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
            m_animation = GetComponent<CultServantAnimation>();
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
