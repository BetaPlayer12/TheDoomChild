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
        [SerializeField,PropertyOrder(-1)]
        private Transform m_locationPoint;

        private void OnValidate()
        {
            if(m_locationPoint == null)
            {
                m_locationPoint = transform;
            }

            m_data?.Set(gameObject.scene, m_locationPoint.position);
            gameObject.name = "LP_" + m_data?.name ?? "NONE";
        }
#endif
    }
}
