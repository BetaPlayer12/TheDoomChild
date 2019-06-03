using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "MultiRayCastData", menuName = "DChild/MultiRayCast Data")]
    public class MultiRayCastData : ScriptableObject
    {
        [SerializeField, MinValue(1), OnValueChanged("UpdateOffset")]
        private int m_count;
#if UNITY_EDITOR
        [SerializeField, MinValue(0.1f), OnValueChanged("UpdateOffset"), ShowIf("ShowWidth")]
        private float m_castWidth;
#endif
        [SerializeField, MinValue(0.1f)]
        private float m_castDistance;
        [SerializeField]
        private LayerMask m_mask;
        [SerializeField]
        private bool m_ignoreTrigger;
        [SerializeField, HideInInspector]
        private float[] m_offsets;

        public int count => m_count;
        public float castDistance => m_castDistance;
        public float[] offsets => m_offsets;
        public LayerMask mask => m_mask;
        public bool ignoreTrigger => m_ignoreTrigger;

        public float[] CalculateOffsets(int castCount, float castWidth)
        {
            if (castCount == 1)
            {
                return new float[] { 0f };
            }
            else
            {
                var offsets = new float[castCount];
                var extent = castWidth / 2;
                var interval = castWidth / (castCount - 1);
                float offset = -extent;
                for (int i = 0; i < m_count; i++)
                {
                    offsets[i] = offset;
                    offset += interval;
                }
                return offsets;
            }
        }

#if UNITY_EDITOR
        private bool ShowWidth() => m_count > 1;
        private void UpdateOffset()
        {
            m_offsets = CalculateOffsets(m_count, m_castWidth);
        }
#endif
    }
}