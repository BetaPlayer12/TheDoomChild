
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Physics;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Gameplay.Systems;

namespace DChild.Gameplay
{
    [System.Serializable]
    public struct ExplosionInfo
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_radius;
        [SerializeField]
        private float m_force;
        [SerializeField]
        private float m_upliftModifier;

        public float radius => m_radius;
        public float force => m_force;
        public float upliftModifier => m_upliftModifier;
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(IIsolatedTime), typeof(IIsolatedPhysicsTime))]
    public abstract class IsolatedPhysics2D : MonoBehaviour, IIsolatedPhysics
    {
        [TabGroup("TabGroup", "Configuration")]
        [SerializeField, ToggleGroup("m_simulateGravity", "Simulate Gravity", GroupID = "TabGroup/Configuration/Gravity")]
        protected bool m_simulateGravity = true;
        [SerializeField, ToggleGroup("m_simulateGravity", GroupID = "TabGroup/Configuration/Gravity"), HideLabel]
        protected GravitySimulator2D m_gravity;

        [SerializeField, ToggleGroup("m_useLinearDrag", "Linear Drag", GroupID = "TabGroup/Configuration/Drag")]
        protected bool m_useLinearDrag;
        [SerializeField, ToggleGroup("m_useLinearDrag", GroupID = "TabGroup/Configuration/Drag"), HideLabel]
        protected LinearDrag m_linearDrag;

        protected Rigidbody2D m_rigidbody2D;
        protected Vector2 m_addedVelocity;
        protected IIsolatedPhysicsTime m_physicsTime;
        public abstract Vector2 velocity { get; protected set; }
        public Vector2 position
        {
            get
            {
                return m_rigidbody2D.position;
            }

            set
            {
                m_rigidbody2D.position = value;
            }
        }

        public float mass => m_rigidbody2D.mass;
        public IGravity2D gravity => m_gravity;
        public ILinearDrag linearDrag => m_linearDrag;
        public RigidbodyType2D bodyType { set { m_rigidbody2D.bodyType = value; } }
        public RigidbodyConstraints2D constraints { get { return m_rigidbody2D.constraints; } set { m_rigidbody2D.constraints = value; } }
        public bool simulateGravity
        {
            get
            {
                return m_simulateGravity;
            }
            set
            {
                m_simulateGravity = value;
            }
        }
        protected abstract Vector2 up { get; }

        public void Enable()
        {
            if (enabled == false)
            {
                m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                SetVelocity(Vector2.zero);
                constraints = RigidbodyConstraints2D.FreezeRotation;
                enabled = true;
            }
        }

        public void Disable()
        {
            if (enabled)
            {
                m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                SetVelocity(Vector2.zero);
                constraints = RigidbodyConstraints2D.FreezeAll;
                enabled = false;
            }
        }

        public void SetVelocity(Vector2 velocity) => this.velocity = m_physicsTime.GetRelativeForce(velocity);

        public void SetVelocity(float x = float.NaN, float y = float.NaN)
        {
            var instanceVelocity = velocity;
            if (m_physicsTime == null)
            {
                AssignValidFloat(ref instanceVelocity.x, x);
                AssignValidFloat(ref instanceVelocity.y, y);
            }
            else
            {
                AssignValidFloat(ref instanceVelocity.x, m_physicsTime.GetRelativeForce(x));
                AssignValidFloat(ref instanceVelocity.y, m_physicsTime.GetRelativeForce(y));
            }
            this.velocity = instanceVelocity;
        }

        public virtual void AddVelocity(Vector2 velocity) => m_addedVelocity += velocity;

        public virtual void AddVelocity(float x = float.NaN, float y = float.NaN)
        {
            AssignValidFloat(ref m_addedVelocity.x, x);
            AssignValidFloat(ref m_addedVelocity.y, y);
        }

        public virtual void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
        {
            m_rigidbody2D.AddForce(m_physicsTime?.GetRelativeForce(force) ?? force, mode);
        }

        public virtual void AddExplosion(float force, Vector2 explosionPosition, float radius, float upliftModified, bool selfImmune = true)
        {
            List<IsolatedPhysics2D> affectedBodies = CastExplosion(this, explosionPosition, radius, selfImmune);
            ApplyExposiveForce(force, explosionPosition, radius, upliftModified, affectedBodies);
        }
        public void AddExplosion(ExplosionInfo explosionInfo, Vector2 explosionPosition, bool selfImmune = true) => AddExplosion(explosionInfo.force, explosionPosition, explosionInfo.radius, explosionInfo.upliftModifier, selfImmune);

        #region Explosion
        private void ApplyExposiveForce(float force, Vector2 explisionPosition, float radius, float upliftModified, List<IsolatedPhysics2D> affectedRigidbodies)
        {
            for (int i = 0; i < affectedRigidbodies.Count; i++)
            {
                //Calculations is taken from 2D Explosion Force Asset
                var body = affectedRigidbodies[i];
                var dir = ((Vector2)body.transform.position - explisionPosition);
                float calc = 1 - (dir.magnitude / radius);
                if (calc <= 0)
                {
                    calc = 0;
                }
                body.AddForce(dir.normalized * force * calc, ForceMode2D.Impulse);
                body.AddForce(up * upliftModified * calc, ForceMode2D.Impulse);
            }
        }

        private List<IsolatedPhysics2D> CastExplosion(IsolatedPhysics2D rigidbody, Vector2 explisionPosition, float radius, bool selfImmune)
        {
            List<IsolatedPhysics2D> affectedRigidbodies = new List<IsolatedPhysics2D>();
            var affectedColliders = Physics2D.OverlapCircleAll(explisionPosition, radius, Physics2D.GetLayerCollisionMask(rigidbody.gameObject.layer));
            for (int i = 0; i < affectedColliders.Length; i++)
            {
                var affectedRigidbody = affectedColliders[i].GetComponentInParent<IsolatedPhysics2D>();
                if (affectedRigidbody != null)
                {
                    if (affectedRigidbody != this || (affectedRigidbody == this && selfImmune == false))
                    {
                        affectedRigidbodies.Add(affectedRigidbody);
                    }
                }
            }

            return affectedRigidbodies;
        }
        #endregion

        private void AssignValidFloat(ref float reference, float value)
        {
            if (!float.IsNaN(value))
            {
                reference = value;
            }
        }

        private void ApplyAddedVelocity()
        {
            m_rigidbody2D.velocity += m_physicsTime?.GetRelativeForce(m_addedVelocity) ?? m_addedVelocity;
            m_addedVelocity = Vector2.zero;
        }


        public virtual void UpdatePhysics()
        {
            ApplyAddedVelocity();
            if (m_simulateGravity)
            {
                m_gravity.Simulate();
            }
            if (m_useLinearDrag)
            {
                velocity = m_linearDrag.ApplyDrag(velocity);
            }
        }


        protected virtual void Awake()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            m_rigidbody2D.gravityScale = 0;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            m_gravity.Initialize(m_rigidbody2D, GetComponent<IIsolatedTime>());
            m_physicsTime = GetComponent<IIsolatedPhysicsTime>();
        }

        private void OnEnable()
        {
            GameplaySystem.world?.Register(this);

        }

        private void OnDisable()
        {
            GameplaySystem.world?.Unregister(this);
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            m_gravity?.DrawDirection();
#endif
        }

#if UNITY_EDITOR
        public void Initialize(float gravityScale)
        {
            var rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            rigidbody2D.gravityScale = 0;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            m_gravity = new GravitySimulator2D();
            m_gravity.Initialize(gravityScale);
        }
#endif
    }
}
