using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantSpider : Minion, IFlinch
    {

        private GiantSpiderAnimation m_animation;
        private SpineRootMotion m_rootMotion;
        private PhysicsMovementHandler2D m_movement;

        protected override Damage startDamage => throw new System.NotImplementedException();

        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(true, true, true);
            m_animation.DoPatrol();
        }

        public void Move()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(true, true, true);
            m_animation.DoMove();
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(false, true, true);
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Hop()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableRootMotion(true, true, true);
            m_behaviour.SetActiveBehaviour(StartCoroutine(HopRoutine()));
        }

        public void Attack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(false, true, true);
            m_behaviour.SetActiveBehaviour(StartCoroutine(AttackRoutine()));
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(false, true, true);
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection damageSource, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(animation.animationState, GiantSpiderAnimation.ANIMATION_DAMAGE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(animation.animationState, GiantSpiderAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator AttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack();
            yield return new WaitForAnimationComplete(animation.animationState, GiantSpiderAnimation.ANIMATION_WEBATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator HopRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoMoveHop();
            yield return new WaitForAnimationComplete(animation.animationState, GiantSpiderAnimation.ANIMATION_MOVEHOP);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(animation.animationState, CombatCharacterAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void EnableRootMotion(bool enable, bool useX, bool useY)
        {
            m_rootMotion.enabled = enable;
            if (enable)
            {
                m_rootMotion.useX = useX;
                m_rootMotion.useY = useY;
            }
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
            m_animation = GetComponent<GiantSpiderAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
