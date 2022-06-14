using DChild.Gameplay.Environment;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    public class NavigationMapManager : MonoBehaviour
    {
        [SerializeField]
        private NavMapInstantiator m_instantiator;
        [SerializeField]
        private NavMapTracker m_tracker;

        private RectTransform m_currentMap;
        private NavMapFogOfWarUI m_fogOfWarUI;
        private bool m_mapNeedsCompleteUpdate = true;

        public void UpdateConfiguration(Location location, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            if (m_instantiator.currentMap != location)
            {
                m_tracker.RemoveUIReferencesFromCurrentMap();
                m_currentMap = m_instantiator.LoadMapFor(location);
                m_fogOfWarUI = GetComponentInChildren<NavMapFogOfWarUI>();
                m_mapNeedsCompleteUpdate = true;
            }

            m_tracker.SetReferencePointPosition(m_currentMap, mapReferencePoint);
            m_tracker.SetInGameTrackReferencePoint(inGameReference);
            m_tracker.SetCalculationOffsets(calculationOffset);
        }

        public void OpenMap()
        {
            if (m_mapNeedsCompleteUpdate)
            {
                m_fogOfWarUI?.UpdateUI();
                m_mapNeedsCompleteUpdate = false;
            }
            else
            {
                var changes = NavigationMapSceneHandle.changes;
                //Only update the ones that needs update

                changes.Clear();
            }

            m_tracker.UpdateTrackerPosition();
            MoveTrackerToCenter();
        }

        private void MoveTrackerToCenter()
        {
            if (m_currentMap == null)
                return;

            m_currentMap.anchoredPosition = -m_tracker.trackerPosition;
        }
    }
}