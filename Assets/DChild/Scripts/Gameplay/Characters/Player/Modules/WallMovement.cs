using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class WallMovement : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        public enum SensorType
        {
            Overhead,
            Body
        }

        [SerializeField, MinValue(0f)]
        private float m_speed;
        [SerializeField]
        private RaySensor m_overheadSensor;
        [SerializeField]
        private RaySensor m_bodySensor;

        private Rigidbody2D m_rigibody;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigibody = info.rigidbody;
        }

        public bool IsThereAWall(SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.Overhead:
                    m_overheadSensor.Cast();
                    return m_overheadSensor.allRaysDetecting && m_overheadSensor.GetProminentHitCollider().gameObject.CompareTag("InvisibleWall") == false;
                case SensorType.Body:
                    m_bodySensor.Cast();
                    return m_bodySensor.allRaysDetecting;
                default:
                    return true;
            }
        }

        public void Cancel()
        {
            m_rigibody.velocity = Vector2.zero;
        }

        public void Move(float direction)
        {
            m_rigibody.velocity = new Vector2(m_rigibody.velocity.x, m_speed * direction);
        }
    }
}
