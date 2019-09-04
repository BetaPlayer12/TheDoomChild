using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [ExecuteInEditMode]
    public class Cursor : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("CursorVisibilityChange")]
        private bool m_makeSystemCursorVisible;

        private void CursorVisibilityChange()
        {
            SetVisibility(m_makeSystemCursorVisible);
        }
#endif
        [SerializeField, OnValueChanged("OnOffsetChange")]
        private Vector2 m_offset;
        [SerializeField]
        private float m_forwardOffset;

        private Vector3 m_offset3D;

        private Camera m_cacheCamera;

        public void SetVisibility(bool isVisible)
        {
            UnityEngine.Cursor.visible = isVisible;
            gameObject.SetActive(isVisible);
        }

        private void OnOffsetChange()
        {
            m_offset3D = new Vector3(m_offset.x, m_offset.y, 0);
        }

        private void Awake()
        {
            m_offset3D = new Vector3(m_offset.x, m_offset.y, 0);
#if UNITY_EDITOR
            SetVisibility(m_makeSystemCursorVisible);
#endif
        }

        private void Start()
        {
            m_cacheCamera = GameSystem.mainCamera;
            GameSystem.CameraChange += OnCameraChange;
        }

        private void OnCameraChange(object sender, CameraChangeEventArgs eventArgs)
        {
            m_cacheCamera = eventArgs.camera;
        }

        public void LateUpdate()
        {
            var position = Input.mousePosition;
            transform.rotation = m_cacheCamera.transform.rotation;
            if (m_cacheCamera.orthographic)
            {
                var worldPosition = m_cacheCamera.ScreenToWorldPoint(position); //We are on the camera's position itself
                worldPosition += m_cacheCamera.transform.forward * m_forwardOffset;
                worldPosition += m_offset3D;
                transform.position = worldPosition;
            }
            else
            {
                Ray ray = m_cacheCamera.ScreenPointToRay(position);
                var cameraForward = m_cacheCamera.transform.forward;
                var point = m_cacheCamera.transform.position + (cameraForward * m_forwardOffset);  //Positioning Plane Infront of Camera;
                Plane xy = new Plane(cameraForward, point);
                float distance;
                xy.Raycast(ray, out distance);
                transform.position = ray.GetPoint(distance) + m_offset3D;
            }
        }
    }
}