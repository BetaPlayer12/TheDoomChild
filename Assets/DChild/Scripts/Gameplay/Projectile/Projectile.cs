﻿using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class Projectile : Actor, IPoolItem, IAttacker
    {
        [SerializeField]
        protected AttackDamage[] m_damage;

        [SerializeField]
        [HideInInspector]
        protected string m_projectileName;

        public event EventAction<CombatConclusionEventArgs> TargetDamaged;
        protected IsolatedPhysics2D m_physics;
        private IIsolatedPhysicsTime m_isolatedPhysicsTime;
        public string projectileName => m_projectileName;

        public void ChangeTrajectory(Vector2 directionNormal) => transform.right = directionNormal;
        public void SetVelocity(Vector2 directionNormal, float speed)
        {
            transform.right = directionNormal;
            m_physics.SetVelocity(directionNormal * speed);
            //m_isolatedPhysicsTime.CalculateActualVelocity();
        }
        public void AddForce(Vector2 force)
        {
            m_physics.AddForce(force);
            transform.right = m_physics.velocity.normalized;
        }

        public void PoolObject() => GameSystem.poolManager.GetOrCreatePool<ProjectilePool>().AddToPool(this);
        public void DestroyItem() => Destroy(gameObject);
        public void SetParent(Transform parent) => transform.parent = parent;

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
        }
#if UNITY_EDITOR
        [SerializeField]
        private bool m_hasConstantSpeed;


#endif

        private void OnValidate()
        {
            m_projectileName = gameObject.name;
#if UNITY_EDITOR
            var physics = GetComponent<IsolatedPhysics2D>();
            physics.simulateGravity = !m_hasConstantSpeed;
#endif
        }
    }
}
