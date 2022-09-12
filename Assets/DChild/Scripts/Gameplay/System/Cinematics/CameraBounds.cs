using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraBounds : MonoBehaviour
    {
        private Camera m_reference;

        private Bounds m_cameraBounds;

        private bool m_prevOrthographic;
        private float m_prevFieldOFView;
        private float m_prevZ;

        public Bounds value => m_cameraBounds;

        private void UpdateCache()
        {
            m_prevOrthographic = m_reference.orthographic;
            m_prevFieldOFView = m_prevOrthographic ? m_reference.orthographicSize : m_reference.fieldOfView;
            m_prevZ = m_reference.transform.position.z;
        }

        private void UpdateCameraBounds()
        {
            if (m_reference.orthographic)
            {
                var vertExtent = m_reference.orthographicSize;
                var horzExtent = vertExtent * Screen.width / Screen.height;
                m_cameraBounds.extents = new Vector3(horzExtent, vertExtent, 0f);
            }
            else
            {
                var frustumHeight = -m_reference.transform.position.z * Mathf.Tan(m_reference.fieldOfView * 0.5f * Mathf.Deg2Rad);
                var frustumWidth = frustumHeight * m_reference.aspect;
                m_cameraBounds.extents = new Vector3(frustumWidth, frustumHeight, 0f);
            }
        }

        private bool HasCameraConfigChanged()
        {
            var configurationChanges = m_reference.orthographic != m_prevOrthographic || (m_prevFieldOFView != (m_prevOrthographic ? m_reference.orthographicSize : m_reference.fieldOfView));
            if (configurationChanges == false && m_reference.orthographic == false)
            {
                return m_prevZ != m_reference.transform.position.z;
            }
            else
            {
                return true;
            }
        }

        private void Awake()
        {
            m_reference = GetComponent<Camera>();
        }

        private void Start()
        {
            UpdateCameraBounds();
            UpdateCache();
        }

        private void LateUpdate()
        {
            var position = transform.position;
            position.z = 0;
            m_cameraBounds.center = position;

            if (HasCameraConfigChanged())
            {
                UpdateCameraBounds();
                UpdateCache();
            }
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.cyan;
            var camBounds = m_cameraBounds;
            Gizmos.DrawWireCube(camBounds.center, camBounds.size);
            Gizmos.color = color;
        }
    }
}