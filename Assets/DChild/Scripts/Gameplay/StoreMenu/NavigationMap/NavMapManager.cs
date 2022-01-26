using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapManager : MonoBehaviour
    {
        [SerializeField]
        private NavMapInstantiator m_instantiator;
        [SerializeField]
        private NavMapFogOfWarUI m_fogOfWarUI;
        [SerializeField]
        private NavMapTracker m_tracker;

        public void UpdateConfiguration(Location location, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            if (m_instantiator.currentMap != location)
            {
                m_tracker.RemoveUIReferencesFromCurrentMap();
                var map = m_instantiator.LoadMapFor(location);
                m_tracker.SetReferencePointPosition(map, mapReferencePoint);
            }
            m_tracker.SetCalculationOffsets(calculationOffset);
        }
    }
}