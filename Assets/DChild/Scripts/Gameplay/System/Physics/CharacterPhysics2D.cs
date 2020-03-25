using Holysoft.Collections;
using DChild.Gameplay.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DChild.Gameplay
{
    public abstract class CharacterPhysics2D : IsolatedPhysics2D
    {
        [TabGroup("TabGroup", "Configuration")]
        [SerializeField, ToggleGroup("m_useCoyoteTime", "Coyote Time", GroupID = "TabGroup/Configuration/Coyote")]
        private bool m_useCoyoteTime;
        [SerializeField, ToggleGroup("m_useCoyoteTime", GroupID = "TabGroup/Configuration/Coyote"), HideLabel]
        private CoyoteTimeModule m_coyoteTime;
        [SerializeField, ToggleGroup("m_useStepClimb", "Step Climb", GroupID = "TabGroup/Configuration/StepClimb")]
        private bool m_useStepClimb = true;
        [SerializeField, ToggleGroup("m_useStepClimb", GroupID = "TabGroup/Configuration/StepClimb"), HideLabel]
        private StepClimber m_stepClimber;
        [SerializeField, TabGroup("TabGroup", "Configuration")]
        private bool m_calculateGroundAngle = true;

        [SerializeField, TabGroup("TabGroup", "Restriction")]
        private RangeFloat m_acceptableWalkableAngle;
        [SerializeField, TabGroup("TabGroup", "References"), ValueDropdown("LegCollisionDropdown")]
        private CollisionDetector m_legCollision;
        [SerializeField, TabGroup("TabGroup", "References"), ValueDropdown("LegColliderDropdown")]
        private Collider2D m_legCollider;

        private ColliderIntersectDetector m_legColliderDetector;
        private float m_groundAngle;

        [ShowInInspector, ReadOnly, TabGroup("TabGroup", "Data")]
        private bool m_onWalkableGround;
        [ShowInInspector, ReadOnly, TabGroup("TabGroup", "Data")]
        private bool m_inContactWithGround;
        [ShowInInspector, ReadOnly, TabGroup("TabGroup", "Data")]
        private Vector2 m_moveAlongGround;
        [SerializeField, TabGroup("TabGroup", "References")]
        private LayerMask m_legColliderLayerMask;
        private RaySensor m_slopeSensor;

        public Vector2 moveAlongGround => m_moveAlongGround;
        public bool onWalkableGround => m_onWalkableGround;
        public bool inContactWithGround => m_inContactWithGround;
        public bool isFalling => m_onWalkableGround == false && velocity.y < -0.1f; //-0.1f;
        public float groundAngle => m_groundAngle;
        public RangeFloat acceptableAngle => m_acceptableWalkableAngle;
        public bool calculateGroundAngle { set => m_calculateGroundAngle = value; }

        public void StopCoyoteTime()
        {
            m_coyoteTime.Stop();
            m_legCollision.ClearCollisions();
            m_onWalkableGround = false;
        }

        public void SetGroundNormal(Vector2 groundNormal)
        {
            m_groundAngle = Vector2.Angle(up, groundNormal);
            m_moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        }

        public void UseStepClimb(bool stepClimb)
        {
            m_useStepClimb = stepClimb;
        }

        public override void UpdatePhysics()
        {
            if (m_calculateGroundAngle)
            {
                CalculateGroundAngle();
            }
            EvaluateGroundedness();
            base.UpdatePhysics();
            if (m_useStepClimb)
            {
                m_stepClimber.Execute(m_rigidbody2D);
            }
        }

        private void UseCoyoteTime()
        {

            if (m_coyoteTime.isAvailable)
            {
                m_onWalkableGround = true;
                m_coyoteTime.Start();
            }
            else
            {
                m_onWalkableGround = false;
            }
        }

        private void CalculateGroundAngle()
        {
            var groundNormal = m_legCollision.collisionCount > 0 ? m_legCollision.GetCollision(0).GetContact(0).normal : up;
            m_groundAngle = Vector2.Angle(up, groundNormal);
            m_moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        }


        private void EvaluateGroundedness()
        {
            //This is where all started 
            if (m_legColliderDetector != null && m_legColliderDetector.IsIntersecting(m_legCollider))
            {
                m_onWalkableGround = false;
                m_inContactWithGround = false;
            }

            if (m_legCollider.IsTouchingLayers(m_legColliderLayerMask) && velocity.y <= 0.1f)
            {
                m_inContactWithGround = true;
                if (m_acceptableWalkableAngle.InRange(m_groundAngle))
                {
                    m_onWalkableGround = true;
                    m_coyoteTime.Reset();
                }
                else
                {
                    if (m_useCoyoteTime)
                    {
                        UseCoyoteTime();
                    }
                    else
                    {
                        m_onWalkableGround = false;
                    }
                }
            }
            else
            {
                m_inContactWithGround = false;
                if (m_useCoyoteTime)
                {
                    UseCoyoteTime();
                }
                else
                {
                    m_onWalkableGround = false;
                }
            }
        }

        protected override void Awake()
        {
            m_inContactWithGround = true;
            m_onWalkableGround = true;
            m_legColliderDetector = GetComponentInChildren<CapsuleColliderDetector>();
            m_stepClimber.Initialize();
            m_legColliderLayerMask = Physics2D.GetLayerCollisionMask(m_legCollider.gameObject.layer);
            base.Awake();
        }

        protected void Update()
        {
            if (m_useCoyoteTime)
            {
                m_coyoteTime.Update(Time.deltaTime);
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            m_stepClimber.DrawGizmos(transform);
        }

        public void Initialize(CollisionDetector legCollision)
        {
            m_legCollision = legCollision;
        }

        public void Initialize(Collider2D legCollider)
        {
            m_legCollider = legCollider;
        }

        public void Initialize(float minWalkableGround, float maxWalkableGround)
        {
            m_acceptableWalkableAngle = new RangeFloat(minWalkableGround, maxWalkableGround);
        }

        private IEnumerable LegCollisionDropdown() => GetComponentsInChildren<CollisionDetector>();
        private IEnumerable LegColliderDropdown()
        {
            var colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
            return colliders.Where(x => x.isTrigger == false);
        }
#endif
    }
}
