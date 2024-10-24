using DChild.Gameplay.Environment;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class WorldTypeManager : MonoBehaviour, IEventActionArgs
    {
        public struct WorldTypeArgs : IEventActionArgs
        {
            public WorldType WorldType;

            public WorldTypeArgs(WorldType currentWorldType)
            {
                WorldType = currentWorldType;
            }
        }

        [SerializeField]
        private WorldType m_currentWorldType;

        public WorldType CurrentWorldType => m_currentWorldType;

        [SerializeField]
        private LocationInWorldData m_underworldLocationsData;
        [SerializeField]
        private LocationInWorldData m_overworldLocationsData;

        public static event EventAction<WorldTypeArgs> OnWorldTypeChanged;

        public void SetCurrentWorldType(Location currentLocation)
        {
            if(m_underworldLocationsData.Locations.Contains(currentLocation) || m_overworldLocationsData.Locations.Contains(currentLocation))
            {
                if (m_underworldLocationsData.Locations.Contains(currentLocation))
                {
                    m_currentWorldType = WorldType.Underworld;
                }
                else
                {
                    m_currentWorldType = WorldType.Overworld;
                }
            }
            else
            {
                m_currentWorldType = WorldType.ArmyBattle;
            }
            

            var args = new WorldTypeArgs(m_currentWorldType);

            OnWorldTypeChanged.Invoke(this, args);
        }
    }
}

