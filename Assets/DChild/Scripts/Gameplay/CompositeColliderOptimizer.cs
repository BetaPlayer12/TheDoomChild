using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class CompositeColliderOptimizer : MonoBehaviour
    {
        [SerializeField]
        private CompositeCollider2D m_compositeCollider2D;

        private void OnBecameInvisible()
        {
            m_compositeCollider2D.generationType = CompositeCollider2D.GenerationType.Manual;
        }

        private void OnBecameVisible()
        {
            m_compositeCollider2D.generationType = CompositeCollider2D.GenerationType.Synchronous;
        }
    }

}