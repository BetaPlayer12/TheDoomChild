using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackBloodFlood : MonoBehaviour
    {
        public bool m_isFlooding;

        [SerializeField]
        private Transform m_floodHeight;
        [SerializeField]
        private float m_floodSpeed;
        private Vector2 m_originalPosition;


        private void Start()
        {
            m_originalPosition = transform.position;
        }

        private void Update()
        {
            if (m_isFlooding)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_floodHeight.position, m_floodSpeed);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, m_originalPosition, m_floodSpeed);
            }
        }


    }

}
