using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackBloodFlood : MonoBehaviour
    {
        public bool isFlooding;

        [SerializeField]
        private Transform m_floodHeight;
        [SerializeField]
        private float m_floodSpeed;
        private Vector2 m_originalPosition;
        [SerializeField]
        private float m_floodDuration;
        private float m_floodDurationValue;


        private void Start()
        {
            m_originalPosition = transform.position;
            m_floodDurationValue = m_floodDuration;
        }

        private void Update()
        {
            if (isFlooding)
            {
                m_floodDuration -= GameplaySystem.time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, m_floodHeight.position, m_floodSpeed);

                if (m_floodDuration < 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, m_originalPosition, m_floodSpeed);
                    isFlooding = false;
                    m_floodDuration = m_floodDurationValue;
                }
            }


        }


    }

}
