using Pathfinding;
using UnityEngine;

namespace DChild.Gameplay.Pathfinding
{
    public class GraphColliderUpdate : MonoBehaviour
    {
        private Collider2D m_collider;

        private GraphUpdateObject m_object;
        private Bounds m_prevBounds;
        private bool m_wasDisabled;
        private bool m_disabledDueToNoAstar;

        private void Start()
        {
            if (AstarPath.active == null)
            {
                m_disabledDueToNoAstar = true;
                enabled = false;
            }
            else
            {
                m_collider = GetComponent<Collider2D>();
                m_prevBounds = m_collider.bounds;
                m_object = new GraphUpdateObject(m_prevBounds);
                m_wasDisabled = gameObject.activeInHierarchy == false;
            }
        }

        private void OnEnable()
        {
            if (m_wasDisabled)
            {
                AstarPath.active?.UpdateGraphs(m_object);
                m_wasDisabled = false;
            }
        }

        private void OnDisable()
        {
            if (m_disabledDueToNoAstar)
                return;

            AstarPath.active?.UpdateGraphs(m_object);
            m_wasDisabled = true;
        }

        private void Update()
        {
            if (m_prevBounds != m_collider.bounds)
            {
                m_prevBounds = m_collider.bounds;
                m_object.bounds = m_prevBounds;
                AstarPath.active?.UpdateGraphs(m_object);
            }
        }
    }
}