using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public sealed class TerrainPatrolSensor : MonoBehaviour
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

        private bool m_shouldTurnAround;

        public bool shouldTurnAround => m_shouldTurnAround;

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