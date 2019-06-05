using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft;
using Holysoft.Event;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class Projectile : PoolableObject, IAttacker
    {
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

        protected IsolatedPhysics2D m_physics;
        private IIsolatedPhysicsTime m_isolatedPhysicsTime;

        protected abstract ProjectileData projectileData { get; }

        public abstract void ResetState();

        public void ChangeTrajectory(Vector2 directionNormal) => transform.right = directionNormal;

        public void SetVelocity(Vector2 directionNormal, float speed)
        {
            transform.right = directionNormal;
            m_physics.SetVelocity(directionNormal * speed);
            //m_isolatedPhysicsTime.CalculateActualVelocity();
        }
        public void AddForce(Vector2 force)
        {
            m_physics.AddForce(force, ForceMode2D.Impulse);
            transform.right = m_physics.velocity.normalized;
        }

        protected void CallAttackerAttacked(CombatConclusionEventArgs eventArgs) => TargetDamaged?.Invoke(this, eventArgs);
        protected bool CollidedWithEnvironment(Collision2D collision) => collision.gameObject.layer == LayerMask.NameToLayer("Environment");
        protected bool CollidedWithEnvironment(Collider2D collision) => CollidedWithSensor(collision) == false && collision.gameObject.layer == LayerMask.NameToLayer("Environment");
        protected bool CollidedWithTarget(Collision2D collision) => collision.gameObject.GetComponentInParent<Hitbox>();
        protected bool CollidedWithTarget(Collider2D collision) => CollidedWithSensor(collision) == false && collision.GetComponentInParent<Hitbox>();
        protected bool CollidedWithSensor(Collider2D collision) => collision.gameObject.CompareTag("Sensor");

        protected virtual void Awake()
        {
            m_physics = GetComponent<IsolatedPhysics2D>();
            m_isolatedPhysicsTime = GetComponent<IIsolatedPhysicsTime>();
            var physics = GetComponent<IsolatedPhysics2D>();
            physics.simulateGravity = !projectileData.hasConstantSpeed;
        }
    }
}
