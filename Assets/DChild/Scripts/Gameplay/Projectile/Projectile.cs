using System;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft;
using Holysoft.Event;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Projectiles
{
    public abstract class Projectile : PoolableObject, IAttacker
    {
        public event EventAction<CombatConclusionEventArgs> TargetDamaged;

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(100), ToggleGroup("m_waitForParticlesEnd", "Destroy On Particle End")]
#endif
        private bool m_waitForParticlesEnd;
        [SerializeField, PropertyOrder(100), ToggleGroup("m_waitForParticlesEnd", "Destroy On Particle End")]
        private ParticleSystem m_particleSystem;
        [SerializeField, PropertyOrder(100), ToggleGroup("m_waitForParticlesEnd", "Destroy On Particle End")]
        private ParticleCallback m_particleCallback;
        [SerializeField, PropertyOrder(100), ToggleGroup("m_waitForParticlesEnd", "Destroy On Particle End")]
        private GameObject m_model;
        [SerializeField, PropertyOrder(101), TabGroup("Launch")]
        private UnityEvent m_onLaunch;
        [SerializeField, PropertyOrder(101), TabGroup("Reset")]
        private UnityEvent m_onReset;


        protected IsolatedPhysics2D m_physics;
        public event EventAction<EventActionArgs> Impacted;

        protected void CallImpactedEvent()
        {
            Impacted?.Invoke(this, EventActionArgs.Empty);
        }

        protected abstract ProjectileData projectileData { get; }

        public bool hasConstantSpeed => projectileData.hasConstantSpeed;

        public IAttacker parentAttacker { get; private set; }

        public IAttacker rootParentAttacker { get; private set; }

        public abstract void ForceCollision();

        public virtual void ResetState()
        {
            if (m_waitForParticlesEnd)
            {
                m_model?.SetActive(true);
                m_particleSystem?.Play();
            }
            m_onReset.Invoke();
        }

        public void ChangeTrajectory(Vector2 directionNormal) => transform.right = directionNormal;

        public void Launch(Vector2 directionNormal, float speed)
        {
            SetVelocity(directionNormal, speed);
            m_onLaunch.Invoke();
        }
        public void SetVelocity(Vector2 directionNormal, float speed)
        {
            transform.right = directionNormal;
            var scale = transform.localScale;
            if (directionNormal.x < 0)
            {
                if (projectileData.isGroundProjectile)
                {
                    if (Mathf.Sign(scale.x) == 1)
                    {
                        scale.x *= -1;
                    }
                }
                else
                {
                    if (Mathf.Sign(scale.y) == 1)
                    {
                        scale.y *= -1;
                    }
                }
            }
            else
            {
                if (projectileData.isGroundProjectile)
                {
                    if (Mathf.Sign(scale.x) == -1)
                    {
                        scale.x *= -1;
                    }
                }
                else
                {
                    if (Mathf.Sign(scale.y) == -1)
                    {
                        scale.y *= -1;
                    }
                }
            }
            transform.localScale = scale;
            m_physics.SetVelocity(directionNormal * speed);
            //m_isolatedPhysicsTime.CalculateActualVelocity();
        }

        public void SetParentAttacker(IAttacker damageDealer)
        {
            parentAttacker = damageDealer;
        }

        public void SetRootParentAttacker(IAttacker damageDealer)
        {
            rootParentAttacker = damageDealer;
        }

        public void AddForce(Vector2 force)
        {
            m_physics.AddForce(force, ForceMode2D.Impulse);
            transform.right = m_physics.velocity.normalized;
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            ResetState();
        }

        public virtual void SetOwner(GameObject gameObject)
        {

        }

        protected void UnloadProjectile()
        {
            if (m_particleSystem == null)
            {
                CallPoolRequest();
            }
            else
            {
                m_model?.SetActive(false);
                m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        private void OnCallback(object sender, EventActionArgs eventArgs)
        {
            CallPoolRequest();
        }



        protected void CallAttackerAttacked(CombatConclusionEventArgs eventArgs) => TargetDamaged?.Invoke(this, eventArgs);
        protected bool CollidedWithEnvironment(Collision2D collision) => collision.gameObject.layer == LayerMask.NameToLayer("Environment");
        protected bool CollidedWithEnvironment(Collider2D collision) => CollidedWithSensor(collision) == false && collision.gameObject.layer == LayerMask.NameToLayer("Environment");
        protected bool CollidedWithTarget(Collision2D collision) => collision.gameObject.GetComponentInParent<Hitbox>();
        protected bool CollidedWithTarget(Collider2D collision) => CollidedWithSensor(collision) == false && collision.GetComponentInParent<Hitbox>();
        protected bool CollidedWithSensor(Collider2D collision) => collision.gameObject.CompareTag("Sensor");

        private void OnParticleSystemStopped()
        {
            CallPoolRequest();
        }

        protected virtual void Awake()
        {
            m_physics = GetComponent<IsolatedPhysics2D>();
            m_waitForParticlesEnd = m_particleSystem;
            if (m_particleCallback)
            {
                m_particleCallback.CallBack += OnCallback;
            }
        }
    }
}
