using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{
    public class UIScrollViewContentOrganizer : ReferenceFactoryAssembler, IScrollViewContentGrid
    {
        [System.Serializable]
        private struct SizeInfo
        {
            [SerializeField]
            [MinValue(0)]
            private float m_width;
            [SerializeField]
            [MinValue(0)]
            private float m_height;

            public float width => m_width;

            public float height => m_height;
        }

        [SerializeField]
        private RectTransform m_content;

        [SerializeField]
        private bool m_restrictInstancePerRow;
        [SerializeField]
        [ShowIf("m_restrictInstancePerRow")]
        [MinValue(1)]
        private int m_instancePerRow = 1;
        [SerializeField]
        private SizeInfo m_instanceSize;
        [SerializeField]
        private SizeInfo m_margin;
        [SerializeField]
        private SizeInfo m_padding;

        public int instancePerRow => m_instancePerRow;

        public bool restrictInstancePerRow => m_restrictInstancePerRow;

        protected override void OnInstanceCreated(object sender, ReferenceInstanceEventArgs eventArgs)
        {
            var rectTransform = eventArgs.value.GetComponent<RectTransform>();
            rectTransform.parent = m_content;
            Organize(rectTransform, eventArgs.referenceIndex);
#if UNITY_EDITOR
            m_items.Add(rectTransform);
#endif
        }

        private void Organize(RectTransform target, int index)
        {
            Vector3 anchorPosition = new Vector3(0, -m_margin.height, m_content.anchoredPosition3D.z);
            UIUtility.SetAnchor(target, HorizontalAnchorType.Left, VerticalAnchorType.Top);
            if (m_restrictInstancePerRow)
            {
                int columnIndex = (int)Mathf.Repeat(index, m_instancePerRow);
                anchorPosition.x = m_margin.width + ((m_padding.width + m_instanceSize.width) * columnIndex);
                int rowIndex = m_instancePerRow == 1 ? index : index / m_instancePerRow;
                anchorPosition.y = -(m_margin.height + ((m_padding.height + m_instanceSize.height) * rowIndex));
            }
            else
            {
                anchorPosition.x = m_margin.width + ((m_padding.width + m_instanceSize.width) * index);
            }
            target.anchoredPosition3D = anchorPosition;
        }

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private List<RectTransform> m_items;

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            if (m_items == null)
            {
                m_items = new List<RectTransform>();
            }
            else
            {
                m_items.Clear();
            }
        }

        public void OrganizeContent()
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                Organize(m_items[i], i);
            }
        }

        [Button]
        private void ClearList()
        {
            m_items.Clear();
        }
#endif

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (m_items != null)
            {
                OrganizeContent();
            }
#endif
        }
    }
}