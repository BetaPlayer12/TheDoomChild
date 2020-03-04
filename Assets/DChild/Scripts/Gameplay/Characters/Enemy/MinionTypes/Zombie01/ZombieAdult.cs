using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieAdult : Minion, IFlinch
    {
        [SerializeField]
        [LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private float m_patrolSpeed;

        private PhysicsMovementHandler2D m_movement;
        private ZombieAdultAnimation m_animation;

        protected override AttackDamage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Patrol(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoWalk();
            m_movement.MoveOnGround(targetPos, m_patrolSpeed);
        }

        public void MoveTo(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoRun();
            m_movement.MoveOnGround(targetPos, m_moveSpeed);
        }

        public void RunAnticipation()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(RunAnticipationRoutine()));
        }

        public void ScratchAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(ScratchAttackRoutine()));
        }

        public void ScratchJumpAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(ScratchJumpAttackRoutine()));
        }

        public void BiteAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(BiteAttackRoutine()));
        }

        public void Detect()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(DetectRoutine()));
        }

        public void Jump()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_behaviour.SetActiveBehaviour(StartCoroutine(JumpRoutine()));
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

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator RunAnticipationRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoRunAnticipation();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_RUN_ANTICIPATION);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator JumpRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoJump();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_JUMP);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDetect();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_DETECT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator BiteAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoBiteAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_BITE_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ScratchAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoScratchAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_SCRATCH_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ScratchJumpAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoScratchJumpAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_SCRATCH_JUMP_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, ZombieAdultAnimation.ANIMATION_DEATH);
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
            m_animation = GetComponent<ZombieAdultAnimation>();
        }
    }
}
