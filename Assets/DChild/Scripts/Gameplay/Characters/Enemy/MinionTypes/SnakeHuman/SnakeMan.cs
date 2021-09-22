using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SnakeMan : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private float m_patrolSpeed;

        [SerializeField]
        private MinionFacing m_facingOnStart;

        private ISensorFaceRotation[] m_sensorRotator;

        private bool m_hasTransistionedTo;
        private SnakeManAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public enum MinionFacing
        {
            left,
            right
        };

        public void Move(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            if (!m_hasTransistionedTo)
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(IdleToMoveRoutine()));
            }
            else
            {
                m_animation.DoMove();
                m_movement.MoveOnGround(targetPos, m_moveSpeed);
            }         
        }

        public void Patrol(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            if (!m_hasTransistionedTo)
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(IdleToMoveRoutine()));
            }
            else
            {
                m_animation.DoMove();
                m_movement.MoveOnGround(targetPos, m_patrolSpeed);
            }
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoIdle();
            m_movement.Stop();
            m_hasTransistionedTo = false;
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void PlayerDetect()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(PlayerDetectRoutine()));
        }

        public void IdleToMove()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(IdleToMoveRoutine()));
        }

        public void VenomAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(VenomAttackRoutine()));
        }

        public void TailAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TailAttackRoutine()));
        }

        public void Flinch(RelativeDirection damageSource, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();

            if (damageSource == RelativeDirection.Back)
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchBackRoutine()));
            }
            else
            {
                m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchFrontRoutine()));
            }
        }

        private IEnumerator FlinchFrontRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDamageFront();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchBackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDamageBack();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_FLINCHBACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TailAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTailAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_TAIL_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator VenomAttackRoutine() //Needs event for venom fx.
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoVenomAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_VENOM_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator IdleToMoveRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoIdleToMove();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_IDLE_TO_MOVE);
            m_hasTransistionedTo = true;
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator PlayerDetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDetectPlayer();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_DETECTPLAYER);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, SnakeManAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            m_hasTransistionedTo = false;
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_animation = GetComponent<SnakeManAnimation>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
            RotateSensor();
        }

        private void OnValidate()
        {
            if (m_facingOnStart == MinionFacing.left)
            {
                SetFacing(HorizontalDirection.Left);
            }
            else
            {
                SetFacing(HorizontalDirection.Right);
            }

            TurnCharacter();
        }
    }
}
