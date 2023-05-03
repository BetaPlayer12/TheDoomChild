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
                for (int i = 0; i < m_colliders.Length; i++)
                {
                    var graphUpdate = new GraphUpdateObject(m_colliders[i].bounds);
                    AstarPath.active?.UpdateGraphs(graphUpdate);
                }
            }
        }

        private IEnumerable GetColliders() => GetComponentsInChildren<Collider2D>();
    }

}