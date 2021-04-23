using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public class SingleFocusHandler : MonoBehaviour
    {
        [SerializeField]
        private UIHighlightHandler[] m_highlightHandler;

        public void FocusOn(UIHighlightHandler highlightHandler)
        {
            for (int i = 0; i < m_highlightHandler.Length; i++)
            {
                if (m_highlightHandler[i] == highlightHandler)
                {
                    m_highlightHandler[i].Highlight();
                }
                else
                {
                    m_highlightHandler[i].Normalize();
                }
            }
        }

        public void UseFocusStateOn(UIHighlightHandler highlightHandler)
        {
            for (int i = 0; i < m_highlightHandler.Length; i++)
            {
                if (m_highlightHandler[i] == highlightHandler)
                {
                    m_highlightHandler[i].UseHighlightState();
                }
                else
                {
                    m_highlightHandler[i].UseNormalizeState();
                }
            }
        }

        public void DontFocusOnAnything()
        {
            for (int i = 0; i < m_highlightHandler.Length; i++)
            {
                m_highlightHandler[i].UseNormalizeState();
            }
        }

#if UNITY_EDITOR
        [Button]
        private void GetChildrenHighlights()
        {
            m_highlightHandler = GetComponentsInChildren<UIHighlightHandler>();
        }
#endif
    }
}