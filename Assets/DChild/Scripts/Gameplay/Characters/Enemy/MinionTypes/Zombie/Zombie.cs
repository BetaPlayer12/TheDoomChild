using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Zombie : Minion
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_jumpForce;

        private PhysicsMovementHandler2D m_movement;
        private IsolatedPhysics2D m_physics;

        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public void PatrolTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_patrolSpeed);
        }

        public void Jump()
        {
            m_physics.SetVelocity(0, m_jumpForce);
        }

        public void UseClaw()
        {

        }

        public void Idle()
        {

        }

        public void Turn()
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
            var oppositeDirection = currentFacingDirection == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left;
            SetFacing(oppositeDirection);
        }

        public void Flinch(params DamageType[] damageTypeRecieved)
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        private IEnumerator Wait()
        {
            m_waitForBehaviourEnd = true;
            yield return new WaitForSeconds(1f);
            StopActiveBehaviour();
        }

        protected override void Awake()
        {
            base.Awake();
            m_physics = GetComponent<IsolatedPhysics2D>();
            m_movement = new PhysicsMovementHandler2D(m_physics, transform);
        }
    }

}