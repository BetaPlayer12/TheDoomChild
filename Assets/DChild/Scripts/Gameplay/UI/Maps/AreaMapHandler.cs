using DChild.Gameplay.UI.Map;
using DChild.Serialization;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class AreaMapHandler : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_content;
        [SerializeField]
        private RectTransform m_playerTracker;
        [SerializeField]
        private RectTransform m_viewPort;

        [SerializeField]
        private DynamicSerializableData m_data;

        [SerializeField, ValueDropdown("GetAreaMaps", IsUniqueList = true)]
        private MapAreaUI[] m_areaUIs;

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

        public void UpdateMap()
        {
            for (int i = 0; i < m_areaUIs.Length; i++)
            {
                m_areaUIs[i].Update(m_data);
            }
        }

        private void Awake()
        {
            m_data.LoadData();
        }

#if UNITY_EDITOR
        private IEnumerable GetAreaMaps() => FindObjectsOfType<MapAreaUI>();
#endif
    }
}