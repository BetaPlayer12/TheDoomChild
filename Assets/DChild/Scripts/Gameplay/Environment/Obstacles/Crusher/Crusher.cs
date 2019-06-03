using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class Crusher : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        private void Update()
        {
            m_rigidbody.velocity = transform.up * -m_speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                m_speed = m_speed * -1;
            }
        }
    }
}