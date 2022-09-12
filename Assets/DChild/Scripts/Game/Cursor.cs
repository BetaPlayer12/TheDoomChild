using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [ExecuteInEditMode]
    [AddComponentMenu("DChild/Misc/Cursor")]
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
        [SerializeField]
        private int m_referenceFeildOfView;

        private Vector3 m_offset3D;

        private Camera m_cacheCamera;
        private Transform m_cacheTransform;
        private Vector3 m_previousMousePosition;
        private Vector3 m_previousCameraPosition;
        private Quaternion m_previousCameraRotation;
        private float m_previousCameraFieldOfView;

        public void SetVisibility(bool isVisible)
        {
            UnityEngine.Cursor.visible = isVisible;
            gameObject.SetActive(isVisible);
            //if (isVisible)
            //{
            //    UnityEngine.Cursor.lockState = CursorLockMode.None;
            //}
            //else
            //{
            //    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            //}
        }

        public void SetLockState(CursorLockMode state)
        {
            UnityEngine.Cursor.lockState = state;
        }

        private void OnOffsetChange()
        {
            m_offset3D = new Vector3(m_offset.x, m_offset.y, 0);
        }

        private void ChangeCamera(Camera camera)
        {
            m_cacheCamera = camera;
            m_cacheTransform = m_cacheCamera?.transform ?? null;

            enabled = m_cacheCamera;
            if (enabled)
            {
                FollowCursor(Input.mousePosition, m_cacheTransform.rotation, m_cacheTransform.position, m_cacheCamera.fieldOfView);
            }
        }

        private void OnCameraChange(object sender, CameraChangeEventArgs eventArgs)
        {
            ChangeCamera(eventArgs.camera);
        }

        private void FollowCursor(Vector3 currentMousePosition, Quaternion currentRotation, Vector3 currentPosition, float currentFieldOfView)
        {
            if (m_previousMousePosition != currentMousePosition || m_previousCameraRotation != currentRotation ||
                m_previousCameraPosition != currentPosition || m_previousCameraFieldOfView != currentFieldOfView)
            {
                transform.rotation = m_cacheTransform.rotation;
                if (m_cacheCamera.orthographic)
                {
                    var worldPosition = m_cacheCamera.ScreenToWorldPoint(currentMousePosition); //We are on the camera's position itself
                    worldPosition += m_cacheTransform.forward * m_forwardOffset;
                    worldPosition += m_offset3D;
                    transform.position = worldPosition;
                }
                else
                {
                    Ray ray = m_cacheCamera.ScreenPointToRay(currentMousePosition);
                    var cameraForward = m_cacheTransform.forward;
                    var point = m_cacheCamera.transform.position + (cameraForward * m_forwardOffset);  //Positioning Plane Infront of Camera;
                    Plane xy = new Plane(cameraForward, point);
                    float distance;
                    xy.Raycast(ray, out distance);
                    Debug.DrawRay(ray.origin, ray.direction * distance);
                    var xOffset = m_cacheTransform.right * m_offset3D.x;
                    var yOffset = m_cacheTransform.up * m_offset3D.y;
                    var worldOffset = ((xOffset + yOffset) * m_cacheCamera.fieldOfView / m_referenceFeildOfView);
                    transform.position = ray.GetPoint(distance) + worldOffset;
                }
                m_previousMousePosition = currentMousePosition;
                m_previousCameraRotation = currentRotation;
                m_previousCameraPosition = currentPosition;
                m_previousCameraFieldOfView = currentFieldOfView;
            }
        }

        private void Awake()
        {
            m_offset3D = new Vector3(m_offset.x, m_offset.y, 0);
            ChangeCamera(GameSystem.mainCamera);
            GameSystem.CameraChange += OnCameraChange;
#if UNITY_EDITOR
            SetVisibility(m_makeSystemCursorVisible);
#endif
        }

        private void LateUpdate()
        {
            if (m_cacheCamera)
            {
                FollowCursor(Input.mousePosition, m_cacheTransform.rotation, m_cacheTransform.position, m_cacheCamera.fieldOfView);
            }
            else
            {
                enabled = false;
            }
        }

        private void OnDestroy()
        {
            GameSystem.CameraChange -= OnCameraChange;
        }
    }
}