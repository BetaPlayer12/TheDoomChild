using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapInstantiator : SerializedMonoBehaviour
    {
        [SerializeField]
        private ScrollRect m_scrollRect;
        [SerializeField]
        private Dictionary<Location, GameObject> m_locationMapPair;

        private Location m_currentLocation = Location._COUNT;
        private RectTransform m_currentMap;

        public Location currentMap => m_currentLocation;

        public RectTransform LoadMapFor(Location location)
        {
            if (m_currentMap != null)
            {
                Destroy(m_currentMap.gameObject);
            }

            var map = Instantiate(m_locationMapPair[location], m_scrollRect.viewport);
            var mapRectTransform = map.GetComponent<RectTransform>();
            m_scrollRect.content = mapRectTransform;
            m_currentLocation = location;

            return mapRectTransform;
        }
    }
}