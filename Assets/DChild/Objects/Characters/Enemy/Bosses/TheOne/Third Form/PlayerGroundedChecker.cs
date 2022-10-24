using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Enemies;

namespace DChild.Gameplay.Characters.AI
{
    public class PlayerGroundedChecker : AggroSensor
    {
        [SerializeField]
        private bool m_playerIsGrounded;
        public bool playerIsGrounded => m_playerIsGrounded;

        private void OnTriggerStay2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IEnemyTarget>();
            if (target != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_playerIsGrounded = true;
                Debug.Log("Player Grounded");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var target = collision.GetComponentInParent<IEnemyTarget>();
            if (target != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                m_playerIsGrounded = false;
                Debug.Log("Player Not Grounded");
            }
        }
    }
}

