using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public struct RayTriggerEventArgs : IEventActionArgs
    {
        public Collider2D collider { get; }

        public RayTriggerEventArgs(Collider2D collider)
        {
            this.collider = collider;
        }
    }

    public class RayTrigger : MonoBehaviour
    {
        public event EventAction<RayTriggerEventArgs> TriggerEnter;

        [SerializeField][MinValue(1)]
        private int m_rayCount;
        [SerializeField]
        private float m_checkWidth;
        [SerializeField]
        private LayerMask m_layerMask;

        private List<Collider2D> m_checkForEnter;
        private RaycastHit2D[] m_hitbuffers;

        [SerializeField]
        [HideInInspector]
        private float[] m_offsets;

        private void Awake()
        {
            m_checkForEnter = new List<Collider2D>();
            m_hitbuffers = new RaycastHit2D[16];
            enabled = false;
        }

        private void FixedUpdate()
        {
            Raycaster.SetLayerMask(m_layerMask);
            var rayOrigin = transform.position;
            int hitCount = 0;
            for (int i = m_checkForEnter.Count - 1; i >= 0; i--)
            {
                var position = m_checkForEnter[i].transform.position;
                for (int j = 0; j < m_rayCount; j++)
                {
                    var castPosition = position;
                    castPosition.y += m_offsets[j];
                    var toRayTarget = castPosition - rayOrigin;
                    m_hitbuffers = Raycaster.Cast(rayOrigin, toRayTarget, true, out hitCount);
                    if(hitCount> 0)
                    {
                        if (m_hitbuffers[0].collider == m_checkForEnter[i])
                        {
                            m_checkForEnter.RemoveAt(i);
                            TriggerEnter?.Invoke(this, new RayTriggerEventArgs(m_checkForEnter[i]));
                            break;
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_checkForEnter.Contains(collision) == false)
            {
                m_checkForEnter.Add(collision);
                if (m_checkForEnter.Count > 0)
                {
                    enabled = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_checkForEnter.Contains(collision))
            {
                m_checkForEnter.Remove(collision);
                if (m_checkForEnter.Count <= 0)
                {
                    enabled = false;
                }
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            CalculateOffsets();
#endif
        }

#if UNITY_EDITOR
        private void CalculateOffsets()
        {
            if (m_rayCount == 1)
            {
                m_offsets = new float[] { 0f };
            }
            else
            {
                m_offsets = new float[m_rayCount];
                var extent = m_checkWidth / 2;
                var interval = m_checkWidth / (m_rayCount - 1);
                float offset = -extent;
                for (int i = 0; i < m_rayCount; i++)
                {
                    m_offsets[i] = offset;
                    offset += interval;
                }
            }
        }
#endif
    }

}