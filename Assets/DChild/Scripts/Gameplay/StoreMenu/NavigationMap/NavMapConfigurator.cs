using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
    [System.Serializable]
    public class NavMapConfigurator
    {
        [SerializeField]
        private Transform m_inGameReferencePoint;
        [SerializeField]
        private Vector3 m_mapReferencePoint;
        [SerializeField]
        private Vector2 m_offset;

        public Transform inGameReferencePoint => m_inGameReferencePoint;
        public Vector3 mapReferencePoint => m_mapReferencePoint;
        public Vector2 offset => m_offset;
    } 
}
