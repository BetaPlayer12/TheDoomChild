using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Gameplay.UI
{
    public class SegmentedStatUI : CappedStatUI
    {
        [System.Serializable]
        public class Segment
        {
            [SerializeField]
            private CappedStatUI m_ui;
            [SerializeField]
            private Canvas m_canvas;
            [SerializeField, MinValue(1)]
            private int m_maxValue = 1;

            public int maxValue => m_maxValue;

            public void Initialize()
            {
                m_ui.maxValue = m_maxValue;
            }

            public void SetActive(bool isActive)
            {
                if (m_canvas)
                {
                    m_canvas.enabled = isActive;
                }
            }

            public void SetCurrentValue(float value)
            {
                m_ui.currentValue = value;
            }
        }

        [SerializeField]
        private Segment[] m_segements;

        public override float maxValue
        {
            set
            {
                var cacheValue = value;
                for (int i = 0; i < m_segements.Length; i++)
                {
                    if (cacheValue <= 0)
                    {
                        m_segements[i].SetActive(false);
                    }
                    else
                    {
                        m_segements[i].SetActive(true);
                        cacheValue -= m_segements[i].maxValue;
                    }
                }
            }
        }

        public override float currentValue
        {
            set
            {
                var cacheValue = value;
                Segment segment = null;
                for (int i = 0; i < m_segements.Length; i++)
                {
                    segment = m_segements[i];
                    if (cacheValue >= segment.maxValue)
                    {
                        segment.SetCurrentValue(segment.maxValue);
                        cacheValue -= segment.maxValue;
                    }
                    else
                    {
                        segment.SetCurrentValue(cacheValue);
                        cacheValue = 0;
                    }
                }
            }
        }

        protected override void Awake()
        {
            for (int i = 0; i < m_segements.Length; i++)
            {
                m_segements[i].Initialize();
            }
            base.Awake();
        }
    }

}