using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapInstantiator : SerializedMonoBehaviour
    {
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

            m_currentLocation = location;

            return null;
        }
    }
}