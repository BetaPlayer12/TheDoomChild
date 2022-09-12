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
        [SerializeField]
        private bool m_useRaycastAll;
        [SerializeField]
        private Collider2D[] m_exceptionList;
        private Vector3 m_originalScale;
        private RaycastHit2D[] m_hitBuffer;
        private RaycastHit2D m_shadowHitRenderBuffer;
        private Vector3 m_prevPosition;

        private void RenderShadow()
        {
            Raycaster.SetLayerMask(DChildUtility.GetEnvironmentMask());
            bool showShadow = false;
            if (m_useRaycastAll)
            {
                m_hitBuffer = Raycaster.CastAll(m_pointOfReference.position, Vector2.down, m_maxDistance);
                for (int i = 0; i < m_hitBuffer.Length; i++)
                {
                    var buffer = m_hitBuffer[i];
                    var collider = buffer.collider;
                    if (collider)
                    {
                        if (collider.isTrigger == false)
                        {
                            if (IsColliderAnException(buffer.collider) == false)
                            {
                                showShadow = true;
                                m_shadowHitRenderBuffer = buffer;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_hitBuffer = Raycaster.Cast(m_pointOfReference.position, Vector2.down, m_maxDistance, true, out int hitcount);
                if (hitcount > 0)
                {
                    showShadow = true;
                    m_shadowHitRenderBuffer = m_hitBuffer[0];
                }
                else
                {
                    showShadow = false;
                }
            }

            if (showShadow)
            {
                if (m_instance.gameObject.activeSelf == false)
                {
                    m_instance.gameObject.SetActive(true);
                }
                UpdateShadowSize(m_shadowHitRenderBuffer);
            }
            else
            {
                m_instance.gameObject.SetActive(false);
            }
        }

        private void UpdateShadowSize(RaycastHit2D raycastHit2D)
        {
            m_instance.position = raycastHit2D.point;
            m_instance.localScale = Vector3.Lerp(Vector3.zero, m_originalScale, 1 - (raycastHit2D.distance / m_maxDistance));
            m_instance.rotation = Quaternion.identity;
        }

        private bool IsColliderAnException(Collider2D collider2D)
        {
            for (int i = 0; i < m_exceptionList.Length; i++)
            {
                if(m_exceptionList[i] == collider2D)
                {
                    return true;
                }
            }

            return false;
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

        private void OnDrawGizmosSelected()
        {
            var referencePosition = m_pointOfReference.position;
            var rayDirection = Vector3.down * m_maxDistance;
            Gizmos.DrawRay(m_pointOfReference.position, rayDirection);
        }
    }
}