using Holysoft;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [System.Serializable]
        public struct Sequence
        {
            [SerializeField]
            private Vector2 m_position;
            [SerializeField]
            [MinValue(0f)]
            private float m_waitDuration;

#if UNITY_EDITOR
            public Sequence(Vector2 m_position)
            {
                this.m_position = m_position;
                m_waitDuration = 0f;
            }
#endif

            public Vector2 position
            {
                get
                {
                    return m_position;
                }
#if UNITY_EDITOR
                set
                {
                    m_position = value;
                }
#endif
            }

            public float waitDuration => m_waitDuration;
        }

        public enum SequenceType
        {
            Loop,
            PingPong
        }

        [SerializeField]
        private SequenceType m_sequenceType;
        [SerializeField]
        [MinValue(0f)]
        private float m_speed;
        [SerializeField]
        [ListDrawerSettings(CustomAddFunction = "CreateSequence")]
        private Sequence[] m_sequences;

        private int m_currentSequenceIndex;
        private Vector2 m_destination;
        private Vector3 m_velocity;
        private bool m_hasReachedDesitantion;
        private float m_waitTimer;

        private bool m_pingPongForward;

        private void UpdateSequenceInformation(int sequenceIndex)
        {
            m_currentSequenceIndex = sequenceIndex;
            var currentSequence = m_sequences[m_currentSequenceIndex];
            m_destination = currentSequence.position;
            m_velocity = ((Vector3)m_destination - transform.position).normalized * m_speed;
            m_hasReachedDesitantion = false;
            m_waitTimer = currentSequence.waitDuration;
        }

        private void HandlePingPongSequence()
        {
            if (m_pingPongForward)
            {
                if (m_currentSequenceIndex == m_sequences.Length - 1)
                {
                    m_pingPongForward = false;
                    UpdateSequenceInformation(m_currentSequenceIndex - 1);
                }
                else
                {
                    UpdateSequenceInformation(m_currentSequenceIndex + 1);
                }
            }
            else
            {
                if (m_currentSequenceIndex == 0)
                {
                    m_pingPongForward = true;
                    UpdateSequenceInformation(1);
                }
                else
                {
                    UpdateSequenceInformation(m_currentSequenceIndex - 1);
                }
            }
        }

        private void Awake()
        {
            transform.position = m_sequences[0].position;
            UpdateSequenceInformation(1);
            m_pingPongForward = true;
        }

        private void Update()
        {
            if (m_hasReachedDesitantion)
            {
                m_waitTimer -= GameplaySystem.time.deltaTime;
                if (m_waitTimer <= 0)
                {
                    if (m_sequenceType == SequenceType.Loop)
                    {
                        UpdateSequenceInformation((int)Mathf.Repeat(m_currentSequenceIndex + 1f, m_sequences.Length));
                    }
                    else if (m_sequenceType == SequenceType.PingPong)
                    {
                        HandlePingPongSequence();
                    }
                }
            }
            else
            {
                transform.position += m_velocity * GameplaySystem.time.deltaTime;
                if (Vector3.Distance(transform.position, m_destination) < 0.1f)
                {
                    m_hasReachedDesitantion = true;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var actor = collision.gameObject.GetComponentInParent<Actor>();
            if (actor)
            {
                actor.transform.parent = transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var actor = collision.gameObject.GetComponentInParent<Actor>();
            if (actor)
            {
                actor.transform.parent = null;
            }
        }
#if UNITY_EDITOR
        [FoldoutGroup("ToolKit")]
        [SerializeField]
        private bool m_useCurrentPosition;
        [FoldoutGroup("ToolKit")]
        [SerializeField]
        [MinValue(0)]
        [ShowIf("m_useCurrentPosition")]
        private int m_overrideSequenceIndex;

        public SequenceType sequenceType => m_sequenceType;
        public Sequence[] sequences => m_sequences;
        public bool useCurrentPosition => m_useCurrentPosition;
        public int overrideSequenceIndex
        {
            get
            {
                return m_overrideSequenceIndex;
            }

            set
            {
                m_overrideSequenceIndex = value;
            }
        }

        private Sequence CreateSequence() => new Sequence(this.transform.position);
        [FoldoutGroup("ToolKit")]
        [Button("Go To Starting Position")]
        private void GoToStartingPosition()
        {
            m_useCurrentPosition = false;
            transform.position = m_sequences[0].position;
            m_overrideSequenceIndex = m_sequences.Length - 1;
        }
#endif
    }
}