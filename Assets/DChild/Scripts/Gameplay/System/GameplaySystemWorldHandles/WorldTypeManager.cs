using DChild.Gameplay.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class WorldTypeManager : MonoBehaviour
    {
        [SerializeField]
        private WorldType m_currentWorldType;

        [SerializeField]
        private LocationInWorldData m_underworldLocationsData;
        [SerializeField]
        private LocationInWorldData m_overworldLocationsData;

        public void SetCurrentWorldType(Location currentLocation)
        {
            if(m_underworldLocationsData.Locations.Contains(currentLocation))
            {
                m_currentWorldType = WorldType.Underworld;
            }
            else
            {
                m_currentWorldType = WorldType.Overworld;
            }
        }
    }
}

