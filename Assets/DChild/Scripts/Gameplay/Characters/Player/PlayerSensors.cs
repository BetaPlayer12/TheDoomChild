using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public class PlayerSensors : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_groundSensor;
        [Space]
        [SerializeField]
        private RaySensor m_feetEdgeSensor;//
        [SerializeField]
        private RaySensor m_kneeSensor;
        [Space]
        [SerializeField]
        private RaySensor m_headSensor;
        [SerializeField]
        private RaySensor m_wallStickSensor;
        [SerializeField]
        private RaySensor m_edgeSensor;
        [SerializeField]
        private RaySensor m_slopeSensor;
        [SerializeField]
        private RaySensor m_groundHeightSensor;
        [Space]
        [SerializeField]
        private RaySensor m_ledgeSensorCliff;//
        [SerializeField]
        private RaySensor m_ledgeSensorEdge;//

        private Vector3 m_leftScale;
        private ISensorFaceRotation[] m_rotatableSensors;

        public PlayerSensors(RaySensor m_groundSensor,RaySensor m_feetEdgeSensor,RaySensor m_kneeSensor ,RaySensor m_headSensor, RaySensor m_edgeSensor, RaySensor m_slopeSensor, RaySensor m_groundHeightSensor, RaySensor m_ledgeSensorCliff, RaySensor m_ledgeSensorEdge)//
        {
            this.m_groundSensor = m_groundSensor;
            this.m_feetEdgeSensor = m_feetEdgeSensor;//
            this.m_kneeSensor = m_kneeSensor;
            this.m_headSensor = m_headSensor;
            this.m_edgeSensor = m_edgeSensor;
            this.m_slopeSensor = m_slopeSensor;
            this.m_groundHeightSensor = m_groundHeightSensor;
            this.m_ledgeSensorCliff = m_ledgeSensorCliff;//
            this.m_ledgeSensorEdge = m_ledgeSensorEdge;//
        }

        public RaySensor groundSensor => m_groundSensor;
        public RaySensor headSensor => m_headSensor;
        public RaySensor wallStickSensor => m_wallStickSensor;
        public RaySensor edgeSensor => m_edgeSensor;
        public RaySensor slopeSensor => m_slopeSensor;
        public RaySensor groundHeightSensor => m_groundHeightSensor;
        public RaySensor ledgeSensorCliff => m_ledgeSensorCliff;//
        public RaySensor ledgeSensorEdge => m_ledgeSensorEdge;//
        public RaySensor feetEdgeSensor => m_feetEdgeSensor;
        public RaySensor kneeSensor => m_kneeSensor;

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
        public void Initialize(RaySensor groundSensor, RaySensor feetEdgeSensor, RaySensor headSensor, RaySensor edgeSensor, RaySensor slopeSensor, RaySensor groundHeightSensor, RaySensor ledgeSensorCliff, RaySensor ledgeSensorEdge)//
        {
            m_groundSensor = groundSensor;
            m_feetEdgeSensor = feetEdgeSensor;
            m_headSensor = headSensor;
            m_edgeSensor = edgeSensor;
            m_slopeSensor = slopeSensor;
            m_groundHeightSensor = groundHeightSensor;
            m_ledgeSensorCliff = ledgeSensorCliff;//
            m_ledgeSensorEdge = ledgeSensorEdge;//
        }

        public void InitializeWallStickSensor(RaySensor wallStickSensor)
        {
            m_wallStickSensor = wallStickSensor;
        }
#endif
    }
}