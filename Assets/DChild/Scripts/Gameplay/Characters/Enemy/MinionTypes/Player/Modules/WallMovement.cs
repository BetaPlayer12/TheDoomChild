using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
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

        private IWallStickState m_state;
        private Rigidbody2D m_rigibody;
        private Animator m_animator;
        private int m_animationParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_rigibody = info.rigidbody;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsWallCrawling);
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
            m_animator.SetBool(m_animationParameter, false);
            m_rigibody.velocity = Vector2.zero;
        }

        public void Move(float direction)
        {
            if (m_state.canWallCrawl == true)
            {
                if (direction != 0)
                {
                    m_animator.SetBool(m_animationParameter, true);
                    m_rigibody.velocity = new Vector2(m_rigibody.velocity.x, m_speed * direction);
                }
            }
        }
    }
}
