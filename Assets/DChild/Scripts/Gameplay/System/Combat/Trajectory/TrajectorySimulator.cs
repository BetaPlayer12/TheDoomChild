using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public class TrajectorySimulator : MonoBehaviour, ITrajectorySimulatorConfig
    {
        [System.Serializable]
        private class CollisionSimulation
        {
            private enum ColliderType
            {
                Box,
                Circle,
                Capsule,
                [HideInInspector]
                None
            }

            [SerializeField]
            [OnValueChanged("OnTypeChange")]
            private ColliderType m_type = ColliderType.None;
            [SerializeField]
            [ShowIf("m_usingBoxCollider")]
            private BoxCollider2D m_boxCollider;
            [SerializeField]
            [ShowIf("m_usingCircleCollider")]
            private CircleCollider2D m_circleCollider;
            [SerializeField]
            [ShowIf("m_usingCapsuleCollider")]
            private CapsuleCollider2D m_capsuleCollider;

            private ContactFilter2D m_contactFilter;
            private RaycastHit2D[] m_hitBuffer;

            public void Initialize()
            {
                m_hitBuffer = new RaycastHit2D[16];
                m_contactFilter = new ContactFilter2D();
                m_contactFilter.useTriggers = false;
                m_contactFilter.useLayerMask = true;
                if (m_type != ColliderType.None)
                {
                    switch (m_type)
                    {
                        case ColliderType.Box:
                            SetContactFilterLayerMask(m_boxCollider.gameObject);
                            break;
                        case ColliderType.Circle:
                            SetContactFilterLayerMask(m_circleCollider.gameObject);
                            break;
                        case ColliderType.Capsule:
                            SetContactFilterLayerMask(m_capsuleCollider.gameObject);
                            break;
                    }
                }
            }

            public void SetColliderToNull()
            {
                m_type = ColliderType.None;
            }

            public void SetCollider(BoxCollider2D collider2D)
            {
                m_type = ColliderType.Box;
                m_boxCollider = collider2D;
                SetContactFilterLayerMask(collider2D.gameObject);
            }

            public void SetCollider(CircleCollider2D collider2D)
            {
                m_type = ColliderType.Circle;
                m_circleCollider = collider2D;
                SetContactFilterLayerMask(collider2D.gameObject);
            }

            public void SetCollider(CapsuleCollider2D collider2D)
            {
                m_type = ColliderType.Capsule;
                m_capsuleCollider = collider2D;
                SetContactFilterLayerMask(collider2D.gameObject);
            }

            private void SetContactFilterLayerMask(GameObject collider)
            {
                m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(collider?.layer ?? LayerMask.NameToLayer("Default")));
            }

            public int Cast(Vector2 position)
            {
                switch (m_type)
                {
                    case ColliderType.Box:
                        return Physics2D.BoxCast(position, m_boxCollider.size, 0, Vector2.zero, m_contactFilter, m_hitBuffer);
                    case ColliderType.Circle:
                        return Physics2D.CircleCast(position, m_circleCollider.radius, Vector2.zero, m_contactFilter, m_hitBuffer);
                    case ColliderType.Capsule:
                        return Physics2D.CapsuleCast(position, m_capsuleCollider.size, m_capsuleCollider.direction, 0, Vector2.zero, m_contactFilter, m_hitBuffer);
                    default:
                        return 0;
                }
            }

#if UNITY_EDITOR
            [SerializeField]
            [HideInInspector]
            private bool m_usingBoxCollider;
            [SerializeField]
            [HideInInspector]
            private bool m_usingCircleCollider;
            [SerializeField]
            [HideInInspector]
            private bool m_usingCapsuleCollider;

            private void OnTypeChange()
            {
                m_usingBoxCollider = m_type == ColliderType.Box;
                m_usingCircleCollider = m_type == ColliderType.Capsule;
                m_usingCapsuleCollider = m_type == ColliderType.Circle;
                m_boxCollider = m_usingBoxCollider ? m_boxCollider : null;
                m_circleCollider = m_usingCircleCollider ? m_circleCollider : null;
                m_capsuleCollider = m_usingCapsuleCollider ? m_capsuleCollider : null;
            }
#endif
        }

        public struct SimulationResultEventArgs : IEventActionArgs
        {
            public SimulationResultEventArgs(List<Vector2> result) : this()
            {
                this.result = result;
            }

            public List<Vector2> result { get; }
        }

        [SerializeField]
        [MinValue(1)]
        [BoxGroup("Simulation")]
        private int m_recordPerFrame;
        [SerializeField]
        [MinValue(1)]
        [BoxGroup("Simulation")]
        private int m_maxRecordSize;

        [SerializeField]
        [BoxGroup("Simulation")]
        private bool m_stopOnCollision;
        [SerializeField]
        [ShowIf("m_stopOnCollision")]
        [BoxGroup("Simulation")]
        [Indent]
        private CollisionSimulation m_collisionSimulation;

        [SerializeField]
        [BoxGroup("Simulation")]
        private bool m_simulatedInsideCameraOnly;
        [SerializeField]
        [ShowIf("m_simulatedInsideCameraOnly")]
        [BoxGroup("Simulation")]
        [Indent]
        private Camera m_camera;

        [SerializeField]
        [BoxGroup("Object")]
        private float m_mass;
        [SerializeField]
        [BoxGroup("Object")]
        private Vector2 m_startPosition;
        [SerializeField]
        [BoxGroup("Object")]
        private bool m_withGravity;
        [SerializeField]
        [BoxGroup("Object")]
        [ShowIf("m_withGravity")]
        [Indent]
        [MinValue(0.0000001f)]
        private float m_gravityScale;
        [SerializeField]
        [BoxGroup("Object")]
        private Vector2 m_velocity;

        public event EventAction<SimulationResultEventArgs> SimulationEnd;

        private List<Vector2> m_simulatedPositions;
        private int m_frameCount;
        private int hitCount;

        public void SetObjectValues(float mass, float gravityScale)
        {
            SetObjectValueInfo(mass, gravityScale);
            m_collisionSimulation.SetColliderToNull();
        }

        public void SetObjectValues(float mass, float gravityScale, BoxCollider2D collider2D)
        {
            SetObjectValueInfo(mass, gravityScale);
            m_collisionSimulation.SetCollider(collider2D);
        }

        public void SetObjectValues(float mass, float gravityScale, CircleCollider2D collider2D)
        {
            SetObjectValueInfo(mass, gravityScale);
            m_collisionSimulation.SetCollider(collider2D);
        }

        public void SetObjectValues(float mass, float gravityScale, CapsuleCollider2D collider2D)
        {
            SetObjectValueInfo(mass, gravityScale);
            m_collisionSimulation.SetCollider(collider2D);
        }

        public void SetObjectValues(float mass, float gravityScale, Collider2D collider2D)
        {
            SetObjectValueInfo(mass, gravityScale);
            var colliderType = collider2D.GetType();
            if(colliderType == typeof(BoxCollider2D))
            {
                m_collisionSimulation.SetCollider((BoxCollider2D)collider2D);
            }
            else if (colliderType == typeof(CircleCollider2D))
            {
                m_collisionSimulation.SetCollider((CircleCollider2D)collider2D);
            }
            else if (colliderType == typeof(CapsuleCollider2D))
            {
                m_collisionSimulation.SetCollider((CapsuleCollider2D)collider2D);
            }
            else
            {
                m_collisionSimulation.SetColliderToNull();
            }
        }

        public void SetVelocity(Vector2 velocity)
        {
            m_velocity = velocity;
        }

        public void SimulateTrajectory()
        {
            m_simulatedPositions.Clear();
            Vector2 simulatedPos = m_startPosition;
            Vector2 currentVelocity = m_velocity;
            hitCount = 0;
            float deltaTime = Time.fixedDeltaTime;
            int recordedCount = 0;
            bool endSimulation = false;
            m_simulatedPositions.Add(simulatedPos);
            m_frameCount = 0;
            do
            {
                simulatedPos = GetNextPosition(simulatedPos, ref currentVelocity, deltaTime);
                RecordPosition(simulatedPos, ref recordedCount, out endSimulation);
            } while (endSimulation == false);
            SimulationEnd?.Invoke(this, new SimulationResultEventArgs(m_simulatedPositions));
        }

        private void SetObjectValueInfo(float mass, float gravityScale)
        {
            m_mass = mass;
            m_withGravity = gravityScale > 0;
            m_gravityScale = gravityScale;
        }

        public void SetStartPosition(Vector2 startPosition) => m_startPosition = startPosition;

        private Vector2 GetNextPosition(Vector2 position, ref Vector2 velocity, float deltaTime)
        {
            if (m_withGravity)
            {
                var deltaGravity = Physics2D.gravity * deltaTime * m_gravityScale;
                velocity += deltaGravity;
            }
            var deltaVelocity = velocity * deltaTime;
            return position + deltaVelocity;
        }

        private void RecordPosition(Vector2 position, ref int recordedCount, out bool endSimulation)
        {
            if (m_stopOnCollision)
            {
                hitCount = m_collisionSimulation.Cast(position);

                if (hitCount > 0)
                {
                    m_simulatedPositions.Add(position);
                    endSimulation = true;
                    return;
                }
            }

            if (m_simulatedInsideCameraOnly)
            {
                if (this.IsVisibleFrom(new Bounds(position, Vector3.zero), m_camera) == false)
                {
                    m_simulatedPositions.Add(position);
                    endSimulation = true;
                    return;
                }
            }

            m_frameCount++;
            if (m_frameCount >= m_recordPerFrame)
            {
                m_frameCount = 0;
                m_simulatedPositions.Add(position);
                recordedCount += 1;
            }
            endSimulation = recordedCount >= m_maxRecordSize;
        }

        private void Awake()
        {
            m_simulatedPositions = new List<Vector2>();
            if (m_stopOnCollision)
            {
                m_collisionSimulation.Initialize();
            }
        }
    }
}