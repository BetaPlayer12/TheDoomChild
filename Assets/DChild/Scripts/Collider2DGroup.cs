using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild
{
    public class Collider2DGroup : MonoBehaviour
    {
        [SerializeField]
        private bool m_canUpdatePathfindingGraph;
        [SerializeField, ValueDropdown("GetColliders", IsUniqueList = true)]
        private Collider2D[] m_colliders;

        private GraphUpdateObject[] m_objectList;

        public void EnableColliders()
        {
            SetColliderEnable(true);
            UpdateGraph();
        }

        public void DisableColliders()
        {
            SetColliderEnable(false);
            UpdateGraph();
        }

        private void SetColliderEnable(bool value)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                m_colliders[i].enabled = value;
            }
        }

        private void UpdateGraph()
        {
            if (m_canUpdatePathfindingGraph)
            {
                for (int i = 0; i < m_objectList.Length; i++)
                {
                    AstarPath.active?.UpdateGraphs(m_objectList[i]);
                }
            }
        }

        private IEnumerable GetColliders() => GetComponentsInChildren<Collider2D>();


        private void Awake()
        {
            if (m_canUpdatePathfindingGraph)
            {
                m_objectList = new GraphUpdateObject[m_colliders.Length];
                for (int i = 0; i < m_objectList.Length; i++)
                {
                    m_objectList[i] = new GraphUpdateObject(m_colliders[i].bounds);
                }
            }
        }
    }

}