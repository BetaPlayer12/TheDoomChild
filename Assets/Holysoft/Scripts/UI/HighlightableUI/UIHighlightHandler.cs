using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{
    public sealed class UIHighlightHandler : UIHighlight
    {
        [SerializeField, OnValueChanged("UpdateHighlight")]
        private bool m_highlight;
        [SerializeField]
        private UIHighlight[] m_highlights;

        public override void Highlight()
        {
            if (m_highlight == false)
            {
                m_highlight = true;
                for (int i = 0; i < m_highlights.Length; i++)
                {
                    m_highlights[i].Highlight();
                }
            }
        }

        public override void Normalize()
        {
            if (m_highlight)
            {
                m_highlight = false;
                for (int i = 0; i < m_highlights.Length; i++)
                {
                    m_highlights[i].Normalize();
                }
            }
        }

        public override void UseHighlightState()
        {
            m_highlight = true;
            for (int i = 0; i < m_highlights.Length; i++)
            {
                m_highlights[i].UseHighlightState();
            }
        }

        public override void UseNormalizeState()
        {
            m_highlight = false;
            for (int i = 0; i < m_highlights.Length; i++)
            {
                m_highlights[i].UseNormalizeState();
            }
        }

        public void Press()
        {
            for (int i = 0; i < m_highlights.Length; i++)
            {
                m_highlights[i].Highlight();
            }
        }

        private void OnEnable()
        {
            if (m_highlight)
            {
                Highlight();
            }
            else
            {
                Normalize();
            }
        }

#if UNITY_EDITOR
        [Button("Get Children Highlights"),HideInPlayMode]
        private void GetChildrenHighlights()
        {
            List<UIHighlight> list = new List<UIHighlight>(GetComponentsInChildren<UIHighlight>());
            list.Remove(this);
            m_highlights = list.ToArray();
        }

        private void UpdateHighlight()
        {
            GetChildrenHighlights();
            if (Application.isPlaying == false)
            {
                if (m_highlight)
                {
                    for (int i = 0; i < m_highlights.Length; i++)
                    {
                        m_highlights[i].UseHighlightState();
                    }
                }
                else
                {
                    for (int i = 0; i < m_highlights.Length; i++)
                    {
                        m_highlights[i].UseNormalizeState();
                    }
                }              
            }
            else
            {
                if (m_highlight)
                {
                    for (int i = 0; i < m_highlights.Length; i++)
                    {
                        m_highlights[i].Highlight();
                    }
                }
                else
                {
                    for (int i = 0; i < m_highlights.Length; i++)
                    {
                        m_highlights[i].Normalize();
                    }
                }
            }
        }
#endif

    }
}