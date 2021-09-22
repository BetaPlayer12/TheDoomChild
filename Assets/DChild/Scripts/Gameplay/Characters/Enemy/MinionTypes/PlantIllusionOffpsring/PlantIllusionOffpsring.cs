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
    public class PlantIllusionOffpsring : Minion, IFlinch, ITerrainPatroller
    {
        [SerializeField]
        private float m_scoutDuration;
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

        private PlantIllusionOffpsringAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;

        private static WaitForWorldSeconds m_scoutWait;
        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private Bone m_boneOverride;

        private Vector2 m_target;

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        public void SetTarget(Vector2 target)
        {
            m_target = target;
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
                m_rootMotion.SetSourceBone("master controller");
                m_animation.SetAnimation(0, "Move", true).TimeScale = !isAggro ? 1 : m_hostileMoveSpeedMult;
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

        public void Burrow()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoBurrow();
            }
        }

        public void BurrowedIdle()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoBurrowedIdle();
            }
        }

        public void BurrowReveal()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoBurrowReveal();
            }
        }
        #endregion

        #region "Attack Behaviors"
        public void JumpAttack()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(JumpAttackRoutine()));
            }
        }

        public void SpitAttack()
        {
            if (Wait())
            {
                StopActiveBehaviour();
                m_behaviour.SetActiveBehaviour(StartCoroutine(SpitRoutine()));
            }
        }
        #endregion

        #region "Conditional Functions"
        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Move")
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
            if (m_animation.GetCurrentAnimation(0).ToString() == "Idle")
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
            yield return new WaitForAnimationComplete(m_animation.animationState, PlantIllusionOffpsringAnimation.ANIMATION_DEATH);
            Destroy(this.gameObject);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, PlantIllusionOffpsringAnimation.ANIMATION_TURN);
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
            yield return new WaitForAnimationComplete(m_animation.animationState, PlantIllusionOffpsringAnimation.ANIMATION_DAMAGE);
            StopActiveBehaviour();
        }

        private IEnumerator JumpAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_movement.Stop();
            m_rootMotion.SetSourceBone("root");
            m_animation.DoJumpPreparation();
            yield return new WaitForAnimationComplete(m_animation.animationState, PlantIllusionOffpsringAnimation.ANIMATION_JUMP_PREPARATION);
            m_animation.DoJumpAttackRoot();
            yield return new WaitForAnimationComplete(m_animation.animationState, PlantIllusionOffpsringAnimation.ANIMATION_JUMP_ATTACK_ROOT);
            m_animation.DoIdle();
            yield return null;
            StopActiveBehaviour();
        }

        private IEnumerator SpitRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_movement.Stop();
            m_animation.DoSpitAttack();
            yield return new WaitForSeconds(0.4f);
            GameObject shoot = Instantiate(m_projectile, m_projectilePos.position, Quaternion.Euler(new Vector3(0, 0, transform.localScale.x == 1 ? 180 : 0)));
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

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_chargeEventName[0])
            {
                //Debug.Log("GAGOO NU GINAGAWA MO");
                Vector3 target = m_target;
                Vector3 v_diff = (target - m_projectilePos.position);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject shoot = Instantiate(m_projectile, m_projectilePos.position, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg));
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
            m_animation = GetComponent<PlantIllusionOffpsringAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_scoutWait = new WaitForWorldSeconds(m_scoutDuration);
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }

            //Spine Event Listener
            //var skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            if (m_animation.skeletonAnimation == null) return;

            m_animation.skeletonAnimation.AnimationState.Event += HandleEvent;

        }
    }
}
