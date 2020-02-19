using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class Collider2DGroup : MonoBehaviour
    {

        [SerializeField, ValueDropdown("GetColliders", IsUniqueList = true)]
        private Collider2D[] m_colliders;

        public void EnableColliders()
        {
            SetColliderEnable(true);
        }

        public void DisableColliders()
        {
            SetColliderEnable(false);
        }

        private void SetColliderEnable(bool value)
        {
            for (int i = 0; i < m_colliders.Length; i++)
            {
                m_colliders[i].enabled = value;
            }
        }

        private IEnumerable GetColliders() => GetComponentsInChildren<Collider2D>();
    }

}