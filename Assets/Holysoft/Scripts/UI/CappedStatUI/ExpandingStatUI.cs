using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Gameplay.UI
{

    public class ExpandingStatUI : CappedStatUI
    {
        [SerializeField]
        private RectTransform m_rectTransform = null;
        [SerializeField, MinValue(0)]
        private float m_baseMaxValue = 0;
        [SerializeField]
        private Vector2 m_padding = new Vector2();
        [SerializeField]
        private Vector2 m_expansionRate = new Vector2();

        private Vector2 m_baseDimension = new Vector2();
        private Vector2 m_stretchDimenension = new Vector2();
        private Vector2 m_anchorDimension = new Vector2();
        private float m_currentMaxValue = 0;

        private Vector2 m_newSizeDelta = new Vector2();

        public override float maxValue
        {
            set
            {
                if (m_currentMaxValue != value)
                {
                    var sizeDelta = m_rectTransform.sizeDelta;
                    var anchorDimensions = new Vector2(m_baseDimension.x - sizeDelta.x, m_baseDimension.y - sizeDelta.y);

                    var difference = value - m_baseMaxValue;
                    var expandedRectSize = m_baseDimension + (m_expansionRate * difference);
                    m_newSizeDelta = expandedRectSize - m_anchorDimension;
                    enabled = true;
                    m_currentMaxValue = value;
                }
            }
        }
        public override float currentValue { set { } }

#if UNITY_EDITOR
        protected override void UpdateUI()
        {
        }

        [Button]
        private void CalculateExpansionRate()
        {
            var rect = m_rectTransform.rect;
            m_baseDimension = new Vector2(rect.width, rect.height);
            m_stretchDimenension = m_baseDimension - m_padding;
            m_expansionRate = m_stretchDimenension / m_baseMaxValue;
        }
#endif

        protected override void Awake()
        {
            m_currentMaxValue = m_baseMaxValue;
            var rect = m_rectTransform.rect;
            m_baseDimension = new Vector2(rect.width, rect.height);
            m_stretchDimenension = m_baseDimension - m_padding;
            var sizeDelta = m_rectTransform.sizeDelta;
            m_anchorDimension = new Vector2(m_baseDimension.x - sizeDelta.x, m_baseDimension.y - sizeDelta.y);
            base.Awake();
            enabled = false;
        }

        private void LateUpdate()
        {
            m_rectTransform.sizeDelta = m_newSizeDelta;
            enabled = false;
        }

        private void OnValidate()
        {
            if (m_rectTransform == null)
            {
                m_rectTransform = GetComponent<RectTransform>();
            }
        }
    }

}