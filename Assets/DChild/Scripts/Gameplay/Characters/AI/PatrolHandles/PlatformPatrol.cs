using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    //Find a way for AI to override AutoRotate while maintaining PatrolHandle

    [AddComponentMenu("DChild/Gameplay/AI/Patrol/Platform Patrol")]
    public class PlatformPatrol : PatrolHandle
    {
        [SerializeField, FoldoutGroup("Reference")]
        private CharacterPhysics2D m_characterPhysics2D;
        [SerializeField, PropertyTooltip("Needs To sense ground to determine if it will detect wall or not"), FoldoutGroup("Reference")]
        private RaySensor m_leadGroundSensor;
        [SerializeField, PropertyTooltip("Senses the edge wall it will snap on to"), FoldoutGroup("Reference")]
        private RaySensor m_edgeWallSensor;
        [SerializeField, PropertyTooltip("Senses the wall it will snap on to"), FoldoutGroup("Reference")]
        private RaySensor m_wallSensor;

        [SerializeField]
        private bool m_autoRotate = true;
        [SerializeField, ShowIf("m_autoRotate"), Indent]
        private Vector2 m_snapOffset;
        [SerializeField, ShowIf("m_autoRotate"), Indent]
        private Transform m_toRotate;

        private bool m_shouldSnapToWall;
        private bool m_shouldSnapToLedge;
        private Vector2 m_snapToPosition;
        private Vector2 m_groundNormal;
        private float m_snapRotation;
        public event EventAction<EventActionArgs> RotateToEdge;
        public event EventAction<EventActionArgs> RotateToWall;

        public override void Patrol(MovementHandle2D movement, float speed, CharacterInfo characterInfo)
        {
            m_characterPhysics2D.simulateGravity = m_characterPhysics2D.GetComponent<Transform>().localRotation.z != 0 ? false : true;

            if (m_shouldSnapToLedge || m_shouldSnapToWall)
            {
                if (m_autoRotate)
                {
                    ExecuteAutoRotate();
                }
            }
            else
            {
                movement.MoveTowards(m_characterPhysics2D.moveAlongGround, speed);
            }
        }

        public override void Patrol(PathFinderAgent agent, float speed, CharacterInfo characterInfo)
        {
            m_characterPhysics2D.simulateGravity = m_characterPhysics2D.GetComponent<Transform>().localRotation.z != 0 ? false : true;

            // I dont really if this works on an Agent
            if (m_shouldSnapToLedge || m_shouldSnapToWall)
            {
                ExecuteAutoRotate();
            }
            else
            {
                agent.Move(speed);
            }
        }

        private void OnWallensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            if (m_wallSensor.isDetecting)
            {
                if (m_autoRotate)
                {
                    m_shouldSnapToWall = true;
                    CalculateSnapValues(m_wallSensor.GetHits()[0]);
                }
                else
                {
                    m_shouldSnapToLedge = true;
                    CalculateSnapValues(m_wallSensor.GetHits()[0]);
                    RotateToWall?.Invoke(this, new EventActionArgs());
                }
            }
            else
            {
                m_shouldSnapToWall = false;
            }
        }

        private void OnGroundSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            if (m_leadGroundSensor.isDetecting == false)
            {
                m_edgeWallSensor.Cast();
                if (m_edgeWallSensor.isDetecting)
                {
                    if (m_autoRotate)
                    {
                        m_shouldSnapToLedge = true;
                        CalculateSnapValues(m_edgeWallSensor.GetHits()[0]);
                    }
                    else
                    {
                        m_shouldSnapToLedge = true;
                        CalculateSnapValues(m_edgeWallSensor.GetHits()[0]);
                        RotateToEdge?.Invoke(this, new EventActionArgs());
                    }
                }
            }
            else
            {
                var normal = m_leadGroundSensor.GetHits()[0].normal;
                if (m_groundNormal != normal)
                {
                    m_characterPhysics2D.SetGroundNormal(normal);
                    m_groundNormal = normal;
                }
                m_shouldSnapToLedge = false;
            }
        }

        public void CalculateSnapValues(RaycastHit2D hit)
        {
            m_snapToPosition = hit.point + m_snapOffset;
            m_groundNormal = hit.normal;
            var groundAngle = new Vector2(m_groundNormal.y, -m_groundNormal.x);
            m_snapRotation = Vector2.SignedAngle(Vector2.right, groundAngle);
        }

        public void ExecuteAutoRotate()
        {
            m_toRotate.position = m_snapToPosition;
            m_toRotate.rotation = Quaternion.Euler(0, 0, m_snapRotation);
            m_characterPhysics2D.SetGroundNormal(m_groundNormal);
            m_shouldSnapToLedge = false;
            m_shouldSnapToWall = false;
        }

        private void Start()
        {
            if (m_leadGroundSensor)
            {
                m_leadGroundSensor.SensorCast += OnGroundSensorCast;
            }
            if (m_wallSensor)
            {
                m_wallSensor.SensorCast += OnWallensorCast;
            }
        }
    }
}