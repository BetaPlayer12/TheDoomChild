using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummySwarmer : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private float m_patrolSpeed;

        private ISensorFaceRotation[] m_sensorRotator;

        private MummySwarmerAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoMove();
            m_movement.MoveOnGround(targetPos, m_patrolSpeed);
        }

        public void Move(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoMove();
            m_movement.MoveOnGround(targetPos, m_moveSpeed);
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

        public void Attack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoAttack();
            m_behaviour.SetActiveBehaviour(StartCoroutine(AttackRoutine()));
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_movement.Stop();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }
        private IEnumerator AttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack();
            yield return new WaitForAnimationComplete(animation.animationState, MummySwarmerAnimation.ANIMATION_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummySwarmerAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummySwarmerAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            RotateSensor();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummySwarmerAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
            gameObject.SetActive(false);
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
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
            m_animation = GetComponent<MummySwarmerAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
        }

    }
}
