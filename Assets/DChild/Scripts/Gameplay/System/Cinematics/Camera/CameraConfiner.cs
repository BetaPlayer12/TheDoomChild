using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class CameraConfiner : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private CompositeCollider2D m_boundingShape2D;

        public CompositeCollider2D boundingShape2D
        {
            get
            {
                return m_boundingShape2D;
            }

#if UNITY_EDITOR
            set
            {
                m_boundingShape2D = value;
            }
#endif
        }

#if UNITY_EDITOR
        [HideInInspector]
        public GameObject m_sensor;
        [HideInInspector]
        public GameObject m_confine;

#endif

        private void OnValidate()
        {
            gameObject.name = $"CameraBounds ({gameObject.scene.name})";
            gameObject.isStatic = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerOnly");
        }
    }
}