using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleGhostMode : MonoBehaviour
    {
        [SerializeField]
        public GameObject m_player;
        [Button]
        public void ToggleOn()
        {
            m_player.GetComponentInChildren<Collider2D>().enabled = false;
            //m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
           
        }

        [Button]
        public void ToggleOff()
        {
            m_player.GetComponentInChildren<Collider2D>().enabled = true;
            //m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 20;
        }

    }
}
