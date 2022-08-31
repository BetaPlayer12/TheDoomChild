using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WeightedPlatform : MonoBehaviour
    {
        [SerializeField]
        private WeightSensor m_weightSensor;
        [SerializeField, MinValue(0.1f)]
        private float m_weightThreshold = 1f;
        [SerializeField]
        private Transform m_platform;
        [SerializeField, HorizontalGroup("StartPosition")]
        private Vector3 m_startingPosition;
        [SerializeField, HorizontalGroup("ActivatedPosition")]
        private Vector3 m_activatedPostion;
        [SerializeField]
        private float m_speed;

        private Vector3 m_destination;
        private float m_startToActivatedPositionSqrMagnitude;
        private bool m_allowMovement = true;

        [ShowInInspector,ReadOnly]
        public float lerpValue { get; private set; }

        public void SetAllowMovement(bool value) => m_allowMovement = value;

        private void OnMassChange(object sender, EventActionArgs eventArgs)
        {
            m_destination = m_weightSensor.currentWeight >= m_weightThreshold ? m_activatedPostion : m_startingPosition;
        }

        private void Awake()
        {
            m_weightSensor.MassChange += OnMassChange;
            m_platform.position = m_startingPosition;
            m_destination = m_startingPosition;

            m_startToActivatedPositionSqrMagnitude = (m_activatedPostion - m_startingPosition).sqrMagnitude;
            lerpValue = 0;
        }

        private void FixedUpdate()
        {
            if (m_allowMovement)
            {
                m_platform.position = Vector3.MoveTowards(m_platform.position, m_destination, m_speed * GameplaySystem.time.fixedDeltaTime);
            }

            var ToCurrentSqrMagnitude = (m_platform.position - m_startingPosition).sqrMagnitude;
            if (ToCurrentSqrMagnitude == 0)
            {
                lerpValue = 0;
            }
            else
            {
                lerpValue = ToCurrentSqrMagnitude / m_startToActivatedPositionSqrMagnitude;
            }
        }

#if UNITY_EDITOR
        [Button("Use Current Position"), HorizontalGroup("StartPosition")]
        private void SetCurrentPositionAsStartPosition() => m_startingPosition = m_platform.position;

        [Button("Use Current Position"), HorizontalGroup("ActivatedPosition")]
        private void SetCurrentPositionAsActivatedPosition() => m_activatedPostion = m_platform.position;
#endif
    }
}