using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PlayerGroundedChecker : MonoBehaviour
    {
        [SerializeField]
        private bool m_playerIsGrounded;
        public bool playerIsGrounded => m_playerIsGrounded;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Character" && collision.gameObject.layer == 8)
            {
                Debug.Log("Player Grounded");
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Character" && collision.gameObject.layer == 8)
            {
                Debug.Log("Player Grounded");
            }
        }
    }
}

