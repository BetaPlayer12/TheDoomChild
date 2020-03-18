using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class Shadow : MonoBehaviour
    {
        [SerializeField]
        private Transform m_pointOfReference;
        [SerializeField]
        private Transform m_instance;
        [SerializeField, MinValue(0.1f)]
        private float m_maxDistance;
        [SerializeField, PropertyTooltip("When this is false it will only calculate when position Changes")]
        private bool m_calculateAlways;
        private Vector3 m_originalScale;
        private RaycastHit2D[] m_hitBuffer;
        private Vector3 m_prevPosition;

        private void RenderShadow()
        {
            Raycaster.SetLayerMask(LayerMask.GetMask("Environment"));
            m_hitBuffer = Raycaster.Cast(m_pointOfReference.position, Vector2.down, m_maxDistance, true, out int hitcount);
            if (hitcount > 0)
            {
                if (m_instance.gameObject.activeSelf == false)
                {
                    m_instance.gameObject.SetActive(true);
                }

                m_instance.position = m_hitBuffer[0].point;
                m_instance.localScale = Vector3.Lerp(Vector3.zero, m_originalScale, 1 - (m_hitBuffer[0].distance / m_maxDistance));
                m_instance.rotation = Quaternion.identity;
            }
            else
            {
                m_instance.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            m_originalScale = m_instance.localScale;
            m_prevPosition = m_pointOfReference.position;
            RenderShadow();
        }

        private void OnDisable()
        {
            m_instance.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (m_prevPosition != m_pointOfReference.position)
            {
                RenderShadow();
                m_prevPosition = m_pointOfReference.position;
            }
        }
    }
}