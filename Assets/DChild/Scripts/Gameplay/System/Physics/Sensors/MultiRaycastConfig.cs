using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "MultiRaycastConfigData", menuName = "DChild/Extras/MultiRaycast Config")]
    public class MultiRaycastConfig : ScriptableObject
    {
        [SerializeField, MinValue(1), OnValueChanged("OnCastWidthChange")]
        private int m_count;
        [SerializeField, MinValue(0.1f)]
        private float m_castDistance;
#if UNITY_EDITOR
        [SerializeField, MinValue(0.1f), OnValueChanged("OnCastWidthChange")]
        private float m_castWidth;
        [SerializeField]
        private bool m_initializeCastConfig;
#endif
        [SerializeField, HideInInspector]
        private float[] m_offsets;

        [SerializeField, ShowIf("m_initializeCastConfig"), Indent]
        private LayerMask m_mask;
        [SerializeField, ShowIf("m_initializeCastConfig"), Indent]
        private bool m_ignoreTrigger;

        public int count => m_count;
        public float[] offsets => m_offsets;
        public float castDistance => m_castDistance;
        public bool ignoreTrigger => m_ignoreTrigger;
        public LayerMask layerMask => m_mask;

        public float[] CalculateOffsets(float width)
        {
            if (m_count == 1)
            {
                return new float[] { 0f };
            }
            else
            {
                var offsets = new float[m_count];
                var extent = m_castWidth / 2;
                var interval = m_castWidth / (m_count - 1);
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
        private void OnCastWidthChange()
        {
           m_offsets = CalculateOffsets(m_castWidth);
        }
#endif
    }
}