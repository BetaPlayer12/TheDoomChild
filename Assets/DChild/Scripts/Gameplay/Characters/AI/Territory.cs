/**************************
 * 
 * Wrapper for Collider to become a territory for Enemies
 * 
 *************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class Territory : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private Collider2D m_area;

        public bool Contains(Vector2 position)
        {
            return m_area.bounds.Contains(position);
        }

        private void OnValidate()
        {
            m_area = GetComponent<Collider2D>();
            m_area.isTrigger = true;
        }
    }
}