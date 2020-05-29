using UnityEngine;

namespace DChild.Gameplay.UI.Map
{

    [System.Serializable]
    public class MapScaler
    {
        [SerializeField]
        private RectTransform m_view;
        [SerializeField]
        private RectTransform m_content;
        [SerializeField]
        private Transform m_tempParent;
        [SerializeField]
        private float m_minScale;
        [SerializeField]
        private float m_maxScale;
        [SerializeField]
        private float m_scaleSpeed;

        public void Update()
        {
            var scrollDelta = Input.mouseScrollDelta.y;
            var viewScale = m_view.localScale;
            var contentScale = m_content.lossyScale;

            switch (Input.mouseScrollDelta.y)
            {
                case float x when (x > 0):
                    if (contentScale.x < m_maxScale)
                    {
                        viewScale += Vector3.one * m_scaleSpeed * x;
                        if ((viewScale.x * contentScale.x) > m_maxScale)
                        {
                            viewScale = Vector3.one * m_maxScale / contentScale.x;
                        }
                        m_view.localScale = viewScale;
                        ResetViewScale();
                    }
                    break;
                case float x when (x < 0):
                    if (contentScale.x > m_minScale)
                    {
                        viewScale += Vector3.one * m_scaleSpeed * x;
                        if ((viewScale.x * contentScale.x) < m_minScale)
                        {
                            viewScale = Vector3.one * m_minScale / contentScale.x;
                        }
                        m_view.localScale = viewScale;
                        ResetViewScale();
                    }
                    break;
            }
        }
        private void ResetViewScale()
        {
            m_content.parent = m_tempParent;
            m_view.localScale = Vector3.one;
            m_content.parent = m_view;
        }
    }
}