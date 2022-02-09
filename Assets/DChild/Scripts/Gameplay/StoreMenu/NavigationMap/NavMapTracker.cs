using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{

    public class NavMapTracker : MonoBehaviour
    {

        [SerializeField]
        private Transform m_toTrack;
        [SerializeField]
        private RectTransform m_uiReferencePoint;
        [SerializeField]
        private RectTransform m_tracker;

        [SerializeField]
        private Vector2 m_calculationOffset;
        [SerializeField]
        private Vector2 m_scaleOffset = Vector2.one;

        private Transform m_inGameTrackReferencePoint;
        public void SetToTrack(Transform toTrack)
        {
            m_toTrack = toTrack;
        }

        public void SetInGameTrackReferencePoint(Transform inGameTrackReferencePoint)
        {
            m_inGameTrackReferencePoint = inGameTrackReferencePoint;
        }

        public void SetReferencePointPosition(RectTransform newMap, Vector2 referencePoint)
        {
            m_uiReferencePoint.SetParent(newMap);
            m_tracker.SetParent(newMap);
            m_uiReferencePoint.anchoredPosition = referencePoint;
        }

        public void SetCalculationOffsets(Vector2 calculationOffset)
        {
            m_scaleOffset = calculationOffset;
        }

        public void RemoveUIReferencesFromCurrentMap()
        {
            m_uiReferencePoint.SetParent(transform);
            m_tracker.SetParent(transform);
        }

        [Button]
        public void UpdateTrackerPosition()
        {
            if (m_inGameTrackReferencePoint)
            {
                var distanceToRefencePoint = (Vector2)(m_toTrack.position - m_inGameTrackReferencePoint.position) * m_scaleOffset;
                m_tracker.anchoredPosition = m_uiReferencePoint.anchoredPosition + distanceToRefencePoint + m_calculationOffset;
            }
        }
    }
}