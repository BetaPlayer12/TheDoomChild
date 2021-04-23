using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class TimedLerpHandler : SerializedMonoBehaviour
    {
        [SerializeField, MinValue(0.1f)]
        private float m_duration = 0.1f;
        [SerializeField, HideReferenceObjectPicker]
        private ILerpHandling[] m_handles = new ILerpHandling[0];

        private float m_timer;

        private float lerpValue => (m_duration - m_timer) / m_duration;

        public void ResetTimer()
        {
            m_timer = m_duration;
            UpdateHandles(lerpValue);
        }

        private void UpdateHandles(float lerpValue)
        {
            for (int i = 0; i < m_handles.Length; i++)
            {
                m_handles[i].SetLerpValue(lerpValue);
            }
        }

        private void Awake()
        {
            m_timer = m_duration;
        }

        private void LateUpdate()
        {
            m_timer -= GameplaySystem.time.deltaTime;
            UpdateHandles(lerpValue);
        }
    }
}