using UnityEngine;

namespace DChild
{
    public class ExpandableComposite2DCollider : MonoBehaviour
    {
        [SerializeField]
        private CompositeCollider2D m_compositeCollider;
        [SerializeField]
        private Collider2D[] m_colliderExpansions;

        public void ActivateExpansions()
        {
            for (int i = 0; i < m_colliderExpansions.Length; i++)
            {
                m_colliderExpansions[i].enabled = true;
            }
            m_compositeCollider.GenerateGeometry();
        }

        public void DeactivateExpansions()
        {
            for (int i = 0; i < m_colliderExpansions.Length; i++)
            {
                m_colliderExpansions[i].enabled = false;
            }
            m_compositeCollider.GenerateGeometry();
        }
    }

}