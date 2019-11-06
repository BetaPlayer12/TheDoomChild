﻿using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ParticleCallback : MonoBehaviour
    {
        public event EventAction<EventActionArgs> CallBack;

        private TrailRenderer m_renderer;
        private int m_previousSegmentCount;

        private void OnParticleSystemStopped()
        {
            CallBack?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_renderer = GetComponent<TrailRenderer>();
            m_previousSegmentCount = 0;
        }

        private void LateUpdate()
        {
            var positionCount = m_renderer.positionCount;
            if (m_previousSegmentCount == 0)
            {
                m_previousSegmentCount = positionCount;
            }
            else if (positionCount == 0)
            {
                m_previousSegmentCount = positionCount;
                CallBack?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private void OnValidate()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                var main = particleSystems[i].main;
#if UNITY_EDITOR
                if (main.stopAction != ParticleSystemStopAction.Callback)
                {
                    main.stopAction = ParticleSystemStopAction.Callback;
                }
#endif
            }
        }
    }
}