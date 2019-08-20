using DChild.Gameplay.Systems.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class LocationPoster : MonoBehaviour
    {
        [SerializeField, InlineEditor(), OnInspectorGUI("OnValidate")]
        private LocationData m_data;

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_data?.Set(gameObject.scene, transform.position);
        }
#endif
    }
}
