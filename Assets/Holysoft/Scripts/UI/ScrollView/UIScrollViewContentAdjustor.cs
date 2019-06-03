using Holysoft.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{

    public sealed class UIScrollViewContentAdjustor : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        [ValidateInput("ValidateContent")]
        private RectTransform m_content;
        [SerializeField]
        [MinValue(0)]
        private float m_minWidth;
        [SerializeField]
        [MinValue(0)]
        private float m_minHeight;

        [PropertySpace()]
        [SerializeField]
        private bool m_hasHorizontalScroll;
        [SerializeField]
        [MinValue(0)]
        [ShowIf("m_hasHorizontalScroll")]
        private float m_widthIncrement = 500;
        [SerializeField]
        private bool m_hasVerticalScroll;
        [SerializeField]
        [MinValue(0)]
        [ShowIf("m_hasVerticalScroll")]
        private float m_heightIncrement = 500;

        private bool ValidateContent(RectTransform content)
        {
            m_minWidth = m_content.rect.width;
            m_minHeight = m_content.rect.height;
            return true;
        }

        private void OnValidate()
        {
            if (m_content != null)
            {
                var contentReference = GetComponent<IReferenceFactoryData>();
                var contentGrid = GetComponent<IScrollViewContentGrid>();
                if (m_hasHorizontalScroll)
                {
                    var contentWidth = m_widthIncrement * (contentGrid.restrictInstancePerRow ? contentGrid.instancePerRow : contentReference.instanceCount);
                    m_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentWidth < m_minWidth ? m_minWidth : contentWidth);
                }
                else
                {
                    m_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_minWidth);

                }
                if (m_hasVerticalScroll && contentGrid.restrictInstancePerRow)
                {
                    int columnCount = contentReference.instanceCount / contentGrid.instancePerRow;
                    var contentHeight = m_heightIncrement * columnCount;
                    m_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight < m_minHeight ? m_minHeight : contentHeight);
                }
                else
                {
                    m_content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_minHeight);
                }
                m_content.ForceUpdateRectTransforms();


            }
        }
#endif
    }
}