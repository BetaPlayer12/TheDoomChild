using Holysoft.Event;
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

        public event EventAction<EventActionArgs> FloodStarted;
        public event EventAction<EventActionArgs> FloodDone;
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
                    isFlooding = false;
                    FloodDone?.Invoke(this, EventActionArgs.Empty);
                    m_floodDuration = m_floodDurationValue;
                }
            }
            if(transform.position != new Vector3(m_originalPosition.x, m_originalPosition.y, 0) && !isFlooding)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_originalPosition, m_floodSpeed);
            }

        }

        public void StartFlooding()
        {
            isFlooding = true;
            FloodStarted?.Invoke(this, EventActionArgs.Empty);
        }
    }

}
