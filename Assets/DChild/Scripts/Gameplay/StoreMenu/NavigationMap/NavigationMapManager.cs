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
        private NavigationMapInstance m_currentInstance;
        [SerializeField]
        private bool m_mapNeedsCompleteUpdate = true;

        public void UpdateConfiguration(Location location, int sceneIndex, Transform inGameReference, Vector2 mapReferencePoint, Vector2 calculationOffset)
        {
            if (m_instantiator.currentMap != location)
            {
                m_tracker.RemoveUIReferencesFromCurrentMap();
                m_currentMap = m_instantiator.LoadMapFor(location);
                m_currentInstance = m_currentMap.GetComponentInChildren<NavigationMapInstance>();
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
                m_currentInstance?.UpdateUI();
                m_mapNeedsCompleteUpdate = false;
            }
            else
            {
                var changes = NavigationMapSceneHandle.changes;
                //Only update the ones that needs update
                for (int i = 0; i < changes.fogOfWarChanges; i++)
                {
                    m_currentInstance.SetUIState(changes.GetFogOfWarName(i), changes.GetFogOfWarState(i));
                }
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