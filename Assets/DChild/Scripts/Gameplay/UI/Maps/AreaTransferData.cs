using DChild.Gameplay.Environment;
using DChild.Gameplay.Systems.Serialization;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    [CreateAssetMenu(fileName = "AreaTransferData", menuName = "DChild/Gameplay/Area Transfer Data")]
    public class AreaTransferData : SerializedScriptableObject
    {
        [System.Serializable]
        public class Data
        {
            [SerializeField]
            private Dictionary<Location, LocationData> m_locationPair = new Dictionary<Location, LocationData>();

            public LocationData GetData(Location location) => m_locationPair.ContainsKey(location) ? m_locationPair[location] : null;

            public List<Location> GetAvailableLocationBranches() => m_locationPair.Keys.ToList();
        }

        [SerializeField]
        private Dictionary<LocationData, Data> m_fromToPair = new Dictionary<LocationData, Data>();

        public List<Location> GetAvailableLocationFrom(LocationData startingPoint)
        {
            if (m_fromToPair.ContainsKey(startingPoint))
            {
                return m_fromToPair[startingPoint].GetAvailableLocationBranches();
            }
            return null;
        }

        public LocationData GetDestination(LocationData from, Location to)
        {
            if (m_fromToPair.ContainsKey(from))
            {
                return m_fromToPair[from].GetData(to);
            }
            return null;
        }
    }
}