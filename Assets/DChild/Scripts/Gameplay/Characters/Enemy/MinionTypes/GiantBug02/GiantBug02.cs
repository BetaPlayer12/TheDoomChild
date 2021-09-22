using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBug02 : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        private GiantBug02Animation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private SpineRootMotion m_rootMotion;
        private RelativeDirection m_damageSource;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoMove();
        }

        public void DetectPlayer()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(DetectPlayerRoutine()));
        }

        public void AcidSpitAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(AcidSpitAttackRoutine()));
        }

        public void JumpAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(JumpAttackRoutine()));
        }

        public void JumpTurn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(JumpTurnRoutine()));
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();

            if (direction == RelativeDirection.Front)
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(FrontFlinchRoutine()));
            }
            else
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(BackFlinchRoutine()));
            }
        }

        private IEnumerator FrontFlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFrontFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, GiantBug02Animation.ANIMATION_FLINCH_FRONT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator BackFlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoBackFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, GiantBug02Animation.ANIMATION_FLINCH_BACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, GiantBug02Animation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator AcidSpitAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAcidSpitAttack();
            yield return new WaitForAnimationComplete(animation.animationState, GiantBug02Animation.ANIMATION_ACID_SPIT_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator JumpAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoPreJumpAttack();
            yield return new WaitForAnimationComplete(animation.animationState, GiantBug02Animation.ANIMATION_PRE_ATTACK);
            m_animation.DoJumpAttack();
            yield return new WaitForAnimationComplete(animation.animationState, GiantBug02Animation.ANIMATION_JUMP_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator JumpTurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoJumpTurn();
            yield return new WaitForAnimationComplete(animation.animationState, GiantBug02Animation.ANIMATION_JUMP_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DetectPlayerRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoPlayerDetect();
            yield return new WaitForAnimationComplete(animation.animationState, GiantBug02Animation.ANIMATION_DETECT_PLAYER);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, GiantBug02Animation.ANIMATION_DEATH);
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
            m_animation = GetComponent<GiantBug02Animation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
        }

        protected override void Start()
        {
            base.Start();
            m_rootMotion.enabled = true;
            m_rootMotion.useX = true;
            m_rootMotion.useY = true;
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
