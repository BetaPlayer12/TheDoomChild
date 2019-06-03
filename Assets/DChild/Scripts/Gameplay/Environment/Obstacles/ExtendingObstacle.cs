using Holysoft;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    [RequireComponent(typeof(IntervalTimer))]
    public class ExtendingObstacle : MonoBehaviour
    {
        [SerializeField]
        private LerpHandler m_extendLerp;

        private IntervalTimer m_intervalTimer;
        [SerializeField]
        [ReadOnly]
        private Vector2 m_retractPosition;
        [SerializeField]
        [ReadOnly]
        private Vector2 m_extendPosition;

        private bool m_shouldExtend;


        private void OnDeactivate(object sender, EventActionArgs eventArgs)
        {
            m_extendLerp.SetDestination(m_retractPosition);
            m_shouldExtend = false;
            enabled = true;
        }

        private void OnActivate(object sender, EventActionArgs eventArgs)
        {
            m_extendLerp.SetDestination(m_extendPosition);
            m_shouldExtend = true;
            enabled = true;
        }

        private void OnDestinationReached(object sender, EventActionArgs eventArgs)
        {
            enabled = false;
        }

        private void Awake()
        {
            m_intervalTimer = GetComponent<IntervalTimer>();
            if (m_intervalTimer.startAsActive)
            {
                transform.position = m_extendPosition;
                m_shouldExtend = true;
            }
            else
            {
                transform.position = m_retractPosition;
                m_shouldExtend = false;
            }
            m_extendLerp.SetTransform(transform);

            m_intervalTimer.Activate += OnActivate;
            m_intervalTimer.Deactivate += OnDeactivate;
            m_extendLerp.DestinationReach += OnDestinationReached;
            enabled = false;
        }

    

        private void Update()
        {
            m_extendLerp.Lerp(GameplaySystem.time.deltaTime);
        }

#if UNITY_EDITOR
        [SerializeField][HideInInspector]
        private bool m_initialized;

        public Vector2 retractPosition { get => m_retractPosition; set => m_retractPosition = value; }
        public Vector2 extendPosition { get => m_extendPosition; set => m_extendPosition = value; }
#endif

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (m_initialized == false)
            {
                m_retractPosition = transform.position;
                m_extendPosition = transform.position;
                m_initialized = true;
            }
#endif
        }
    }

}