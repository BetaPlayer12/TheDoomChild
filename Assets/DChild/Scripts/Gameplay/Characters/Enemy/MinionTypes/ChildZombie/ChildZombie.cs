using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ChildZombie : Minion
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        private Transform m_projectileSpawn;
        [SerializeField]
        private GameObject m_projectileVomit;
        [SerializeField]
        private GameObject m_vomit;

        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;

        private PhysicsMovementHandler2D m_movement;
        private bool m_isScratching;

        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void Idle()
        {
            m_movement.Stop();

        }

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public void PatrolTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_patrolSpeed);
        }

        public void Vomit()
        {
            m_movement.Stop();
            Debug.Log("Vomit");
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        public void ProjectileVomit(Vector2 position)
        {
            m_movement.Stop();
            var projectileGO = this.InstantiateToScene(m_projectileVomit, m_projectileSpawn.position, m_projectileSpawn.rotation);
            var direction = (position - (Vector2)m_projectileSpawn.position);
            var projectile = projectileGO.GetComponent<AttackProjectile>();
            projectile.ChangeTrajectory(direction.normalized);
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        public void Scratch()
        {
            m_movement.Stop();
            Debug.Log("Scratch");
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        public void Turn()
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
            var oppositeDirection = currentFacingDirection == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left;
            SetFacing(oppositeDirection);
        }

        public void SenseSurrounding()
        {
            m_movement.Stop();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Wait()));
        }

        private void Flinch(params DamageType[] damageTypeRecieved)
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


        protected void ResetValue()
        {
            Idle();
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
        }
    }

}