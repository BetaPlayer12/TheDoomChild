using DChild.Gameplay;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [System.Serializable]
    public class AutoReflexHandler : IAutoReflexHandler
    {
        [SerializeField, MinValue(0f)]
        private float m_slowValue;
        [SerializeField, MinValue(0.1f)]
        private float m_duration;
        private CountdownTimer m_timer;
        private bool m_enabled;

        public event EventAction<EventActionArgs> AutoReflexEnd
        {
            add
            {
                m_timer.CountdownEnd += value;
            }

            remove
            {
                m_timer.CountdownEnd -= value;
            }
        }

        [Button]
        public void StartAutoReflex()
        {
            m_timer.SetStartTime(m_duration);
            m_timer.Reset();
            m_enabled = true;
            GameplaySystem.world.SetTimeScale(m_slowValue);
        }
        [Button]
        public void StopAutoReflex()
        {
            m_timer.Reset();
            m_enabled = false;
            GameplaySystem.world.SetTimeScale(1);
        }

        public void Update(float deltaTime)
        {
            if (m_enabled)
            {
                m_timer.Tick(deltaTime);
            }
        }

        public void Initialize()
        {
            m_timer = new CountdownTimer(0);
            m_timer.CountdownEnd += OnDurationEnd;
        }

        private void OnDurationEnd(object sender, EventActionArgs eventArgs)
        {
            StopAutoReflex();
        }
    } 
}
