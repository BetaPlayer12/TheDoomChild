using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;


namespace DChild.Gameplay
{
    public class IntervalTimer : MonoBehaviour
    {
        public event EventAction<EventActionArgs> Activate;
        public event EventAction<EventActionArgs> Deactivate;

        [SerializeField]
        [BoxGroup("State")]
        protected CountdownTimer m_activeTime;
        [SerializeField]
        [BoxGroup("State")]
        protected CountdownTimer m_inactiveTime;
        [SerializeField]
        [BoxGroup("State")]
        [PropertyOrder(100)]
        private bool m_startAsActive;

        [SerializeField]
        private bool m_hasInTime;
        [SerializeField]
        [ShowIf("m_hasInTime")]
        [MinValue(0)]
        [OnValueChanged("InTimeChange")]
        private float m_inTime;

        private bool m_isUsingInTime;
        private bool m_isActive;
        private float m_originalStartTime;

        public bool startAsActive { get => m_startAsActive; }

        protected virtual void OnInactiveEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_isUsingInTime)
            {
                m_inactiveTime.SetStartTime(m_originalStartTime);
                m_isUsingInTime = false;
            }
            SwitchToActive();
        }

        private void SwitchToActive()
        {
            m_activeTime.Reset();
            Activate?.Invoke(this, EventActionArgs.Empty);
            m_isActive = true;
        }

        protected virtual void OnActiveEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_isUsingInTime)
            {
                m_activeTime.SetStartTime(m_originalStartTime);
                m_isUsingInTime = false;
            }
            SwitchToDeactive();
        }

        private void SwitchToDeactive()
        {
            m_inactiveTime.Reset();
            Deactivate?.Invoke(this, EventActionArgs.Empty);
            m_isActive = false;
        }

        protected virtual void UpdateTimers(float deltaTime)
        {
            if (m_isActive)
            {
                m_activeTime.Tick(deltaTime);
            }
            else
            {
                m_inactiveTime.Tick(deltaTime);
            }
        }

        private void SetTimerToInTime(CountdownTimer timer, float inTime)
        {
            m_originalStartTime = timer.startTime;
            timer.SetStartTime(m_originalStartTime - inTime);
            m_isUsingInTime = true;
        }

        protected virtual void Start()
        {
            if (m_startAsActive)
            {
                if (m_hasInTime)
                {
                    SetTimerToInTime(m_activeTime, m_inTime);
                }
                SwitchToActive();
            }
            else
            {
                if (m_hasInTime)
                {
                    SetTimerToInTime(m_inactiveTime, m_inTime);
                }
                SwitchToDeactive();
            }

            m_activeTime.CountdownEnd += OnActiveEnd;
            m_inactiveTime.CountdownEnd += OnInactiveEnd;
        }

        private void Update()
        {
            UpdateTimers(GameplaySystem.time.deltaTime);
        }

#if UNITY_EDITOR
        private void InTimeChange()
        {
            if (m_startAsActive)
            {
                m_inTime = Mathf.Min(m_inTime, m_activeTime.startTime);
            }
            else
            {
                m_inTime = Mathf.Min(m_inTime, m_inactiveTime.startTime);
            }
        }
#endif
    }

}