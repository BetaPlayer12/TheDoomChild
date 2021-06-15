using DChild.Gameplay.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class DestroySpawnArea : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_minion;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
           

            if (collision.gameObject.transform.parent.gameObject.name == m_minion.name+"Model")
            {
                collision.gameObject.GetComponentInParent<Damageable>().TakeDamage(999999, AttackType.True);
            }
        }
    }
}
