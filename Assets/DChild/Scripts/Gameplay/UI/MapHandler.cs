using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DChild.Gameplay.UI
{
    public class MapHandler : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_content;
        [SerializeField]
        private RectTransform m_playerTracker;
        [SerializeField]
        private RectTransform m_viewPort;

        //Handle Which maps to show
        // On Show try to center playerMarker

        public void CenterTrackedPlayer()
        {
            var contentParent = m_content.parent;
            var trackerParent = m_playerTracker.parent;

            m_playerTracker.parent = m_viewPort;
            m_content.parent = m_playerTracker;
            m_playerTracker.anchoredPosition = Vector2.zero;
            m_content.parent = m_viewPort;
            m_playerTracker.parent = m_content;
        }
    }
}