using DChild.Gameplay.Systems.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class LocationPoster : MonoBehaviour
    {
        [SerializeField, InlineEditor(), OnInspectorGUI("OnValidate")]
        private LocationData m_data;

        public LocationData data { get => m_data; }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private Transform m_locationPoint;
        [SerializeField, HideInInspector]
        private Vector3 m_prevPosition;

        private void OnValidate()
        {
            if (m_locationPoint == null)
            {
                m_locationPoint = transform;
            }

            if (Application.isPlaying == false)
            {
                var currentPosition = m_locationPoint.position;
                if (m_prevPosition != currentPosition)
                {
                    m_data?.Set(gameObject.scene, currentPosition);
                    m_prevPosition = currentPosition;
                }
            }
            if (gameObject.scene.name != null)
            {
                var newName = "LP_" + m_data?.name ?? "NONE";
                if (gameObject.name != newName)
                {
                    gameObject.name = newName;
                }
            }
        }
#endif
    }
}
