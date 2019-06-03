using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public class PlayerSensors : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_groundSensor;
        [SerializeField]
        private RaySensor m_headSensor;
        [SerializeField]
        private RaySensor m_wallStickSensor;
        [SerializeField]
        private RaySensor m_edgeSensor;
        [SerializeField]
        private RaySensor m_slopeSensor;

        private Vector3 m_leftScale;
        private ISensorFaceRotation[] m_rotatableSensors;

        public PlayerSensors(RaySensor m_groundSensor, RaySensor m_headSensor, RaySensor m_edgeSensor, RaySensor m_slopeSensor)
        {
            this.m_groundSensor = m_groundSensor;
            this.m_headSensor = m_headSensor;
            this.m_edgeSensor = m_edgeSensor;
            this.m_slopeSensor = m_slopeSensor;
        }

        public RaySensor groundSensor => m_groundSensor;
        public RaySensor headSensor => m_headSensor;
        public RaySensor wallStickSensor => m_wallStickSensor;
        public RaySensor edgeSensor => m_edgeSensor;
        public RaySensor slopeSensor => m_slopeSensor;

        public void SetDirection(HorizontalDirection facing)
        {
            transform.localScale = facing == HorizontalDirection.Left ? m_leftScale : Vector3.one;

            for (int i = 0; i < m_rotatableSensors.Length; i++)
            {
                m_rotatableSensors[i].AlignRotationToFacing(facing);
            }
        }

        public void Start()
        {
            m_leftScale = new Vector3(-1, 1, 1);
            m_rotatableSensors = GetComponentsInChildren<ISensorFaceRotation>();
        }

#if UNITY_EDITOR
        public void Initialize(RaySensor groundSensor, RaySensor headSensor, RaySensor edgeSensor, RaySensor slopeSensor)
        {
            m_groundSensor = groundSensor;
            m_headSensor = headSensor;
            m_edgeSensor = edgeSensor;
            m_slopeSensor = slopeSensor;
        }

        public void InitializeWallStickSensor(RaySensor wallStickSensor)
        {
            m_wallStickSensor = wallStickSensor;
        }
#endif
    }
}