using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class MovableCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;
        [SerializeField, MinValue(1)]
        private float m_minZ;
        [SerializeField, MinValue(1)]
        private float m_maxZ;

        [SerializeField, MinValue(1)]
        private float m_zoomSpeed;
        [SerializeField, MinValue(1)]
        private float m_moveSpeed;
        [SerializeField, MinValue(1)]
        private float m_boost;

        public void Update()
        {
            if (m_camera != null)
            {
                m_camera.fieldOfView = 55;

                var moveSpeed = m_moveSpeed * Time.unscaledDeltaTime * (1 + (Input.GetKey(KeyCode.M) ? m_boost : 0));

                var position = m_camera.transform.position;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    position += (Vector3.up * moveSpeed);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    position += (Vector3.down * moveSpeed);
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    position += (Vector3.left * moveSpeed);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    position += (Vector3.right * moveSpeed);
                }

                var zoomSpeed = m_zoomSpeed * Time.unscaledDeltaTime * (1 + (Input.GetKey(KeyCode.M) ? m_boost : 0));
                if (Input.GetKey(KeyCode.Comma))
                {
                    position += (Vector3.forward * zoomSpeed);
                }
                else if (Input.GetKey(KeyCode.Period))
                {
                    position += (Vector3.back * zoomSpeed);

                }

                var threshold = -1 * m_minZ;
                if (position.z > threshold)
                {
                    position.z = threshold;
                    m_camera.transform.position = position;
                }

                threshold = -1 * m_maxZ;
                if (position.z < threshold)
                {
                    position.z = threshold;
                    m_camera.transform.position = position;
                }

                m_camera.transform.position = position;
            }
        }


    }
}