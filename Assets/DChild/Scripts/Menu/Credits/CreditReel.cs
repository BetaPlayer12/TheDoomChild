using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Menu
{
    public class CreditReel : UIAnimation, ICreditReel
    {
        [SerializeField]
        private RectTransform m_target;
        [SerializeField]
        [MinValue(0.1)]
        private float m_speed;
        private Vector3 m_initialPosition;
        private Vector3 m_speedIncrement;

        public event EventAction<EventActionArgs> CreditsPlay;
        public event EventAction<EventActionArgs> CreditsPause;
        public event EventAction<EventActionArgs> CreditsStop;

        public override void Pause()
        {
            enabled = false;
            CreditsPause?.Invoke(this, EventActionArgs.Empty);
        }
        public override void Play()
        {
            m_startOnAwake = true;
            enabled = true;
            CreditsPlay?.Invoke(this, EventActionArgs.Empty);
        }

        public override void Stop()
        {
            m_startOnAwake = false;
            enabled = false;
            m_target.anchoredPosition3D = m_initialPosition;
            CreditsStop?.Invoke(this, EventActionArgs.Empty);
        }

        protected override void Awake()
        {
            base.Awake();
            m_initialPosition = m_target.anchoredPosition3D;
        }

        private void LateUpdate()
        {
            m_speedIncrement.y = m_speed * Time.deltaTime;
            m_target.anchoredPosition3D += m_speedIncrement;
        }

        private void OnValidate()
        {
            if(m_target == null)
            {
                m_target = GetComponent<RectTransform>();
            }
        }
    }
}