using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Butcher : Minion, IFlinch
    {
        [SerializeField]                                                                                                                                                                                              
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;

        [SerializeField]
        private Damage m_damage;

        private PhysicsMovementHandler2D m_movement;
        private ButcherAnimation m_animation;

        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void MoveTo(Vector2 position)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoMove();
            m_movement.MoveOnGround(position, m_moveSpeed);
        }

        public void PatrolTo(Vector2 position)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoMove();
            m_movement.MoveOnGround(position, m_patrolSpeed);
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoIdle();
            m_movement.Stop();
        }

        public void Slash()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SlashAttackRoutine()));
        }

        public void HeavySlash()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(HeavySlashAttackRoutine()));
        }

        public void SlashChop()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(SlashChopAttackRoutine()));
        }

        public void Turn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDamage();
            yield return null;
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, ButcherAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SlashAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSlashAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ButcherAnimation.ANIMATION_SLASH_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator HeavySlashAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoHeavySlashAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ButcherAnimation.ANIMATION_HEAVYSLASH_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator SlashChopAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoSlashChopAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ButcherAnimation.ANIMATION_SLASH_CHOP_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, ButcherAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
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
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_animation = GetComponent<ButcherAnimation>();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}