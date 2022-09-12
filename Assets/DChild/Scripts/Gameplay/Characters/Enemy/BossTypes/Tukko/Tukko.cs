using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using Spine.Unity.Modules;
using UnityEngine;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Tukko : BossTemplate, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        private TukkoAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private ITurnHandler m_turn;
        private PhysicsMovementHandler2D m_movement;


        private static WaitForWorldSeconds m_animationBlendWait;
        private static bool m_isStaticInitialized;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
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

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoMove();
            }
        }

        #region "Basic Behaviors"
        public void Turn()
        {
            //StopActiveBehaviour();
            //m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
            TurnCharacter();
        }

        public void HopBackwards()
        {
            //if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
            //{
            //}
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_animation.DoHopBackwards();
        }

        public void JumpUp()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoJumpUp();
            }
        }

        public void Damage()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoDamage();
            m_animation.DoDamageAnim();
        }

        public void AirDamage()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = true;
            m_animation.DoDamage();
            m_animation.DoAirDamage();
        }
        #endregion

        #region "Attack Behaviors"
        public void AirborneGrenades()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoAttack3AirborneGrenades();
            }
        }

        public void Jab()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttackJab();
            }
        }

        public void StabFull()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = false;
                m_animation.DoAttack1StabFull();
            }
        }

        public void AirborneSlam()
        {
            if (Wait())
            {
                m_rootMotion.enabled = true;
                m_rootMotion.useY = true;
                m_animation.DoAttack4AirborneSlam();
            }
        }

        public void SmokeBomb()
        {
            m_rootMotion.enabled = true;
            m_rootMotion.useY = false;
            m_animation.DoSmokeBomb();
        }

        public void ShadowGrenades()
        {
            m_animation.DoAttack3AirborneGrenades();
            m_animation.skeletonAnimation.AnimationState.GetCurrent(0).AnimationStart = 0.6f;
        }

        public void ShadowSlam()
        {
            m_animation.DoAttack4AirborneSlam();
            m_animation.skeletonAnimation.AnimationState.GetCurrent(0).AnimationStart = 0.6f;
        }
        #endregion

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

        public bool IsThrowingSmoke()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Smoke_Bomb")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsInvisible()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Smoke_Bomb")
            {
                if (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
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

        public bool IsThrownBomb()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == "Attack3_Airborne_Grenades")
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

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, TukkoAnimation.ANIMATION_DEATH);
            StopActiveBehaviour();
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoIdle();
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
            yield return null;
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_rootMotion.enabled = false;
            m_movement.Stop();
            //m_animation.DoDamage();
            m_animation.DoDamageAnim();
            yield return new WaitForAnimationComplete(m_animation.animationState, TukkoAnimation.ANIMATION_DAMAGE);
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
            m_animation = GetComponent<TukkoAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();

            if (m_isStaticInitialized == false)
            {
                m_animationBlendWait = new WaitForWorldSeconds(0.2f);
                m_isStaticInitialized = true;
            }

        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, AttackSummaryInfo attackInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
