using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Holysoft
{
    public sealed class DelayedEvent : MonoBehaviour
    {
        [SerializeField]
        private CountdownTimer m_delay;
        [SerializeField]
        private bool m_startOnAwake;
        [SerializeField]
        private UnityEvent m_event;

        public void StartEvent()
        {
            m_delay.Reset();
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_event?.Invoke();
        }

        private void Awake()
        {
            m_delay.Reset();
            m_delay.CountdownEnd += OnCountdownEnd;
            enabled = m_startOnAwake;
        }

        private void Update()
        {
            m_delay.Tick(Time.deltaTime);
        }
    }

}