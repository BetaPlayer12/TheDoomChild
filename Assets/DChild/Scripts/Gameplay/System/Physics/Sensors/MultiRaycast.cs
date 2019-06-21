﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class MultiRaycast
    {
        [SerializeField, InlineEditor]
        private MultiRayCastData m_config;
#if UNITY_EDITOR
        [SerializeField]
        private Color m_rayColor = Color.green;
#endif
        private int m_count;
        private float m_castDistance;
        private LayerMask m_mask;
        private bool m_ignoreTrigger;
        private float[] m_offsets;

        public bool isDetecting { get; private set; }
        public bool areAllRaysDetecting { get; private set; }
        public RaycastHit2D[] hits { get; private set; }
        public int count => m_count;

        public void Initialize()
        {
            m_count = m_config.count;
            m_castDistance = m_config.castDistance;
            m_offsets = m_config.offsets;
            m_mask = m_config.mask;
            m_ignoreTrigger = m_config.ignoreTrigger;
            hits = new RaycastHit2D[m_count];
        }

        public void Set(int rayCount, float castWidth, float castLength)
        {
            if (m_count != rayCount)
            {
                m_offsets = new float[m_count];
                hits = new RaycastHit2D[m_count];
                m_count = rayCount;
            }
            m_offsets = m_config.CalculateOffsets(rayCount, castWidth);
            m_castDistance = castLength;
        }

        public void SetCastDistance(float value) => m_castDistance = value;

        public void SetCast(LayerMask mask, bool ignoreTrigger)
        {
            m_mask = mask;
            m_ignoreTrigger = ignoreTrigger;
        }

        public void UseConfiguration()
        {
            m_count = m_config.count;
            m_castDistance = m_config.castDistance;
            m_offsets = m_config.offsets;
            m_mask = m_config.mask;
            m_ignoreTrigger = m_config.ignoreTrigger;
            hits = new RaycastHit2D[m_count];
        }

        public void Cast(Vector2 origin, Vector2 direction)
        {
            direction = direction.normalized;
            isDetecting = false;
            areAllRaysDetecting = false;
            Raycaster.SetLayerMask(m_mask);
            int hitCount = 0;
            RaycastHit2D[] hitBuffers;
            var detectionCount = 0;
            var offsetDirection = new Vector2(direction.y, -direction.x);
            for (int i = 0; i < m_count; i++)
            {
                var position = origin + (offsetDirection * m_offsets[i]);
                hitBuffers = Raycaster.Cast(position, direction, m_castDistance, m_ignoreTrigger, out hitCount);
                hits[i] = hitBuffers[0];

                if (hitCount > 0)
                {
                    if (isDetecting == false)
                    {
                        isDetecting = true;
                    }

                    detectionCount++;
                }
            }

            if (detectionCount == m_count)
            {
                areAllRaysDetecting = true;
            }
        }


#if UNITY_EDITOR
        public void DrawGizmo(Vector2 position, Vector2 relativeUp, Vector2 relativeRight)
        {
            Gizmos.color = m_rayColor;
            if (Application.isPlaying)
            {
                for (int i = 0; i < m_count; i++)
                {
                    Gizmos.DrawRay(position + (relativeUp * m_offsets[i]), relativeRight * m_castDistance);
                }
            }
            else
            {
                if (m_config != null)
                {
                    for (int i = 0; i < m_config.count; i++)
                    {
                        Gizmos.DrawRay(position + (relativeUp * m_config.offsets[i]), relativeRight * m_config.castDistance);
                    }
                }
            }
        }
#endif
    }
}