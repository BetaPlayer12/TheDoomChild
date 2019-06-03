using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public struct LerpDuration
    {
        [SerializeField, MinValue(0), OnValueChanged("OnDurationChange")]
        private float m_duration;
        [SerializeField, HideInEditorMode, ReadOnly]
        private float m_lerpSpeed;
        private bool m_assumeLerp;
        private float m_lerpValue;

        public LerpDuration(float m_duration) : this()
        {
            this.m_duration = m_duration;
            m_lerpSpeed = 1 / m_duration;
            m_assumeLerp = duration == 0;
            m_lerpValue = 0;
        }

        public float duration => m_duration;
        public float lerpValue => m_lerpValue;

        public void Update(float deltaTime)
        {
            if (m_assumeLerp)
            {
                m_lerpValue = Mathf.Sign(deltaTime) > 0 ? 1 : 0;
            }
            else
            {
                m_lerpValue = Mathf.Clamp01(m_lerpValue + (m_lerpSpeed * deltaTime));
            }
        }

        public void SetValue(float value) => m_lerpValue = Mathf.Clamp01(value);
        public void SetDuration(float duration)
        {
            m_duration = duration;
            m_lerpSpeed = 1 / m_duration;
            m_assumeLerp = duration == 0;
        }

        public void SetSpeed(float speed)
        {
            m_lerpSpeed = speed;
            m_assumeLerp = false;
        }

#if UNITY_EDITOR
        private void OnDurationChange() => m_lerpSpeed = 1 / m_duration;
#endif
    }
}