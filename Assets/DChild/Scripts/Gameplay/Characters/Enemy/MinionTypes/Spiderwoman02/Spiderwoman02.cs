using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies.Collections;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Spiderwoman02 : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
        [SerializeField]
        private float m_moveSpeed;
        [SerializeField]
        private float m_hostileMoveSpeedMult;
        [SerializeField]
        private Transform m_cannonTF;
        [SerializeField]
        private GameObject m_projectileGO;

        [SerializeField]
        private Damage m_damage;

        private Spiderwoman02Animation m_animation;
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

        public void SetState()
        {
            m_model.localScale = new Vector3(1, 1, 1);
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
                m_animation.DoWalk();
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
        //        m_rootMotion.enabled = true;
        //        m_rootMotion.useY = true;
        //        m_animation.SetAnimation(0, "Walk", true).TimeScale = m_hostileMoveSpeedMult;
        //        //m_movement.MoveOnGround(target, ((m_moveSpeed * m_hostileMoveSpeedMult) * transform.localScale.x));
        //    }
        //}

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
                m_animation.SetAnimation(0, "Walk", true).TimeScale = !isAggro ? 1 : m_hostileMoveSpeedMult;
                //m_movement.MoveTo(direction, m_moveSpeed);
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

        public void MoveBackwards(bool isAggro)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (m_animation.GetCurrentAnimation(0).ToString() != "Attack")
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.SetAnimation(0, "Walk_Backwards", true).TimeScale = !isAggro ? 1 : m_hostileMoveSpeedMult;
            }
        }
        #endregion

        #region "Attack Behaviors"
        public void Attack()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(ShootRoutine()));
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Walk")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Walk")
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
            m_animation.DoDeathWithRed();
            yield return new WaitForAnimationComplete(m_animation.animationState, Spiderwoman02Animation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, Spiderwoman02Animation.ANIMATION_TURN);
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
            yield return new WaitForAnimationComplete(m_animation.animationState, Spiderwoman02Animation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        private IEnumerator ShootRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoAttack();
            yield return new WaitForSeconds(1.3f);
            GameObject shoot = Instantiate(m_projectileGO, m_cannonTF.position, Quaternion.Euler(new Vector3(0, 0, transform.localScale.x == 1 ? 135 : 45)));
            //Debug.Log("Boom Explode");
            yield return new WaitForAnimationComplete(m_animation.animationState, Spiderwoman02Animation.ANIMATION_ATTACK);
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
            m_movement = new PhysicsMovementHandler2D(GetComponent<ShiftingCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<Spiderwoman02Animation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }
        }
    }
}
