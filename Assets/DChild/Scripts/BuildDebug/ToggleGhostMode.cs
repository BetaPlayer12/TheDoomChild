using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleGhostMode : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField]
        public GameObject m_player;

        private bool m_isOn;

        public bool value => m_isOn;

        [Button]
        public void ToggleOn()
        {
            m_isOn = true;
            m_player.GetComponentInChildren<Collider2D>().enabled = false;
            m_player.GetComponentInChildren<Damageable>().SetHitboxActive(false);
            //m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 0;
        }

        [Button]
        public void ToggleOff()
        {
            m_isOn = false;

            m_player.GetComponentInChildren<Collider2D>().enabled = true;
            m_player.GetComponentInChildren<Damageable>().SetHitboxActive(true);
            //m_player.GetComponentInChildren<Rigidbody2D>().gravityScale = 20;
        }

    }
}
