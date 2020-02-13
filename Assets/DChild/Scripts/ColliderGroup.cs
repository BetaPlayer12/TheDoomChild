using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class Collider2DGroup : MonoBehaviour
    {

        [SerializeField,ValueDropdown("GetColliders",IsUniqueList = true)]
        private Collider2D[] m_colliders;

        private IEnumerable GetColliders() => GetComponentsInChildren<Collider2D>();
    }

}