using DChild.Gameplay.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "LocationInWorldData")]
    public class LocationInWorldData : ScriptableObject
    {
        [SerializeField]
        private WorldType m_worldType;

        [SerializeField]
        private List<Location> m_locations;

        public WorldType WorldType => m_worldType;
        public List<Location> Locations => m_locations;
    }
}

