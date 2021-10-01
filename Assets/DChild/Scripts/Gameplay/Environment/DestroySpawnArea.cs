using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
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

        private int m_minionID;
        private void Start()
        {
            m_minionID = m_minion.GetComponent<Character>().ID;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                if (collision.gameObject.TryGetComponentInParent(out Character character))
                {
                    if (character.ID == m_minionID)
                    {
                        //collision.gameObject.GetComponentInParent<PoolableObject>().CallPoolRequest();
                        collision.gameObject.GetComponentInParent<Damageable>().TakeDamage(999999, DamageType.True);
                    }
                }
            }
        }
    }
}
