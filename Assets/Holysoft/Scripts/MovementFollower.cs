using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Gameplay
{
    public class MovementFollower : MonoBehaviour
    {
        [SerializeField]
        private Transform m_ToFollow;
        [SerializeField]
        private bool m_invertX;
        [SerializeField]
        private bool m_invertY;
        [SerializeField]
        private bool m_invertZ;

        private Vector3 m_prevToFollowPosition;

        private void OnEnable()
        {
            m_prevToFollowPosition = m_ToFollow.position;
        }

        private void LateUpdate()
        {
            var currentPosition = m_ToFollow.position;
            if (currentPosition != m_prevToFollowPosition)
            {
                var movement = currentPosition - m_prevToFollowPosition;
                if (m_invertX)
                {
                    movement.x *= -1;
                }
                if (m_invertY)
                {
                    movement.y *= -1;
                }
                if (m_invertZ)
                {
                    movement.z *= -1;
                }
                transform.position += movement;
                m_prevToFollowPosition = currentPosition;
            }
        }
    }
}