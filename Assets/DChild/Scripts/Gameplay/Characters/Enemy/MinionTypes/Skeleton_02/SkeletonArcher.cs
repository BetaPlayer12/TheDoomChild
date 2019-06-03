using DChild;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonArcher : Minion, IFlinch
    {
        [SerializeField]
        [LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        private SkeletonArcherAnimation m_animation;
        private EnemyFacingOnStart m_enemyFacing;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoIdle();
        }

        public void Attack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(AttackRoutine()));
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, AttackType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonArcherAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonArcherAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator AttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonArcherAnimation.ANIMATION_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonArcherAnimation.ANIMATION_DEATH);
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
            m_animation = GetComponent<SkeletonArcherAnimation>();
            m_enemyFacing = GetComponent<EnemyFacingOnStart>();
            m_enemyFacing.enabled = true;
        }
    }
}
