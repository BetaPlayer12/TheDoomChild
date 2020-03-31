using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild
{
    public class SpriteCycle : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("OnListChange")]
        private SpriteList m_list;
        [SerializeField]
        private SpriteRenderer m_renderer;
        [SerializeField, HideInInspector]
        private int m_currentIndex;

        private void OnListChange()
        {
            if (m_list != null && m_renderer != null)
            {
                if (m_currentIndex > m_list.count - 1)
                {
                    m_currentIndex = 0;
                }

                m_renderer.sprite = m_list.GetSprite(m_currentIndex);
            }
        }

        [Button, ShowIf("@m_list != null && m_renderer != null"), HorizontalGroup("Button")]
        private void Previous()
        {
            m_currentIndex = (int)Mathf.Repeat(m_currentIndex - 1, m_list.count);
            m_renderer.sprite = m_list.GetSprite(m_currentIndex);
        }

        [Button, ShowIf("@m_list != null && m_renderer != null"), HorizontalGroup("Button")]
        private void Next()
        {
            m_currentIndex = (int)Mathf.Repeat(m_currentIndex + 1, m_list.count);
            m_renderer.sprite = m_list.GetSprite(m_currentIndex);
        }
#endif
    }
}