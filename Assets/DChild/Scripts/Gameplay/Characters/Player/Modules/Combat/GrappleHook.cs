﻿using DChild.Gameplay.Environment;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class GrappleHook : MonoBehaviour
    {
        public struct ObjectCatchedEventArgs : IEventActionArgs
        {
            public ObjectCatchedEventArgs(IGrappleObject grappleObject)
            {
                position = grappleObject.position;
                canBePulled = grappleObject.canBePulled;
                pullOffset = grappleObject.pullOffset;
                dashOffset = grappleObject.dashOffset;
            }

           public Vector2 position { get; }
           public bool canBePulled { get; }
           public float pullOffset { get; }
           public float dashOffset { get; }
        }

        public event EventAction<ObjectCatchedEventArgs> ObjectCatched;
        public event EventAction<EventActionArgs> HookReturned;

        [SerializeField, MinValue(1f)]
        private float m_maxLength;
        [SerializeField, MinValue(1f)]
        private float m_speed;
        [SerializeField]
        private Collider2D m_collider;

        private bool m_gotoDirection;
        private bool m_goBack;
        private Vector2 m_direction;
        private Vector2 m_launchPosition;
        private Vector2 m_initialLocalPosition;
        private IGrappleObject m_catchedObject;

        public IGrappleObject catchedObject => m_catchedObject;

        public void ResetState()
        {
            transform.localPosition = m_initialLocalPosition;
            m_collider.enabled = false;
        }

        public void Launch(Vector2 direction)
        {
            m_gotoDirection = true;
            m_direction = direction;
            m_launchPosition = transform.position;
            m_collider.enabled = true;
            enabled = true;
        }

        private void Awake()
        {
            enabled = false;
        }

        public void Update()
        {
            if (m_gotoDirection)
            {
                transform.position += (Vector3)(m_direction * m_speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, m_launchPosition) >= m_maxLength)
                {
                    m_gotoDirection = false;
                    m_goBack = true;
                    m_collider.enabled = false;
                    m_direction = (m_launchPosition - (Vector2)transform.position).normalized;
                }
            }
            else if (m_goBack)
            {
                transform.position += (Vector3)(m_direction * m_speed * Time.deltaTime);
                if (Vector2.Distance(transform.position, m_launchPosition) < 0.5f)
                {
                    m_goBack = false;
                    HookReturned?.Invoke(this, EventActionArgs.Empty);
                    enabled = false;
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Grappable"))
            {
                m_catchedObject = collision.GetComponent<IGrappleObject>();
                ObjectCatched?.Invoke(this, new ObjectCatchedEventArgs(m_catchedObject));
                m_gotoDirection = false;
                m_collider.enabled = false;
                transform.position = m_catchedObject.position;
            }
        }
    }
}