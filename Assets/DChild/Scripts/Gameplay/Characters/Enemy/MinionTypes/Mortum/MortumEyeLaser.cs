using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MortumEyeLaser : MonoBehaviour
    {
        [SerializeField]
        private float m_maxDistance;
        [SerializeField]
        private Transform m_endPoint;
        [SerializeField]
        private EdgeCollider2D m_collider;

        [SerializeField]
        private ParticleSystem[] m_fxs;


        private List<Vector2> m_edgeColliderPoints;

        public void SetMaxDistance(float maxDistance)
        {
            m_maxDistance = maxDistance;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < m_fxs.Length; i++)
            {
                m_fxs[i].Play();
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            for (int i = 0; i < m_fxs.Length; i++)
            {
                m_fxs[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        private void OnCharacterTurn(object sender, FacingEventArgs eventArgs)
        {
            var rotation = transform.rotation.eulerAngles;
            if (eventArgs.currentFacingDirection == HorizontalDirection.Left)
            {
                rotation.z += 180;
            }
            else
            {
                rotation.z -= 180;
            }
            transform.rotation = Quaternion.Euler(rotation);
        }

        private void Awake()
        {
            m_edgeColliderPoints = new List<Vector2>();
            GetComponentInParent<Character>().CharacterTurn += OnCharacterTurn;
        }

        private void Update()
        {
            m_edgeColliderPoints.Clear();
            int hitCount = 0;

            var hits = Raycaster.Cast(transform.position, transform.right, true, out hitCount, true);
            if (hitCount == 0)
            {
                var laserEndPoint = transform.position + (transform.right * m_maxDistance);
                m_endPoint.position = laserEndPoint;
            }
            else
            {
                m_endPoint.position = hits[0].point;
            }

            m_edgeColliderPoints.Add(Vector2.zero);

            m_edgeColliderPoints.Add(transform.InverseTransformPoint(m_endPoint.position));
            m_collider.SetPoints(m_edgeColliderPoints);
        }

        private void OnDrawGizmosSelected()
        {
            var laserEndPoint = transform.position + (transform.right * m_maxDistance);
            Gizmos.DrawLine(transform.position, laserEndPoint);
        }
    }
}

