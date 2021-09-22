using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonSpider : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;

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

        public void ClimbUp()
        {

        }

        public void ClimbDown()
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

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        private IEnumerator Wait()
        {
            m_waitForBehaviourEnd = true;
            yield return new WaitForSeconds(1f);
            m_behaviour.SetActiveBehaviour(null);
            m_waitForBehaviourEnd = false;
        }
    }

}