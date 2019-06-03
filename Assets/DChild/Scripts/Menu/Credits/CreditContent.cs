using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu
{
    public class CreditContent : UICanvas
    {
        [SerializeField]
        [ReadOnly]
        private RectTransform m_rectTransform;
        public RectTransform rectTransform => m_rectTransform;

        private void OnValidate()
        {
            m_canvas = GetComponent<Canvas>();
            m_rectTransform = GetComponent<RectTransform>();
            UIUtility.SetPivot(m_rectTransform, HorizontalAnchorType.Center, VerticalAnchorType.Bottom);
        }
    }
}