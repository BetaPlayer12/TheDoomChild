using UnityEngine;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.AI
{
    [AddComponentMenu("DChild/Gameplay/AI/Patrol/Terrain Patrol")]
    public class TerrainPatrol : PatrolHandle
    {
        [System.Serializable]
        public class Sensors
        {
            [SerializeField]
            private RaySensor m_sensor;
            [SerializeField]
            private bool m_turnWhenDetection = true;

            public RaySensor sensor => m_sensor;
            public bool shouldTurnAround => m_sensor.isDetecting == m_turnWhenDetection;
        }

        [SerializeField]
        private Sensors[] m_sensors;

        private Vector2 m_moveDirection;
        private bool m_directionFound;
        private bool m_shouldTurnAround;

        public override void Patrol(MovementHandle2D movement, float speed, CharacterInfo characterInfo)
        {
            if (m_shouldTurnAround)
            {
                CallTurnRequest();
                m_directionFound = false;
            }
            else
            {
                if (m_directionFound == false)
                {
                    m_moveDirection = characterInfo.currentFacing == HorizontalDirection.Left ? Vector2.left : Vector2.right;
                    m_directionFound = true;
                }
                movement.MoveTowards(m_moveDirection, speed);
            }
        }

        public override void Patrol(PathFinderAgent agent, float speed, CharacterInfo characterInfo)
        {
            if (m_shouldTurnAround)
            {
                CallTurnRequest();
                m_directionFound = false;
            }
            else
            {
                if (m_directionFound == false)
                {
                    m_moveDirection = characterInfo.currentFacing == HorizontalDirection.Left ? Vector2.left : Vector2.right;
                    m_directionFound = true;
                }
                agent.MoveTowardsForced(m_moveDirection, speed);
            }
        }

        private void OnSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            m_shouldTurnAround = false;
            for (int i = 0; i < m_sensors.Length; i++)
            {
                if (m_sensors[i].shouldTurnAround)
                {
                    m_shouldTurnAround = true;
                    break;
                }
            }
        }

        private void Start()
        {
            for (int i = 0; i < m_sensors.Length; i++)
            {
                m_sensors[i].sensor.SensorCast += OnSensorCast;
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < m_sensors.Length; i++)
            {
                m_sensors[i].sensor.SensorCast -= OnSensorCast;
            }
        }
    }
}