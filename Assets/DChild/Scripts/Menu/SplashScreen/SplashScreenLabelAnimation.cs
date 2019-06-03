using Holysoft.UI;
using UnityEngine;

namespace DChild.Menu.MainMenu
{
    public class SplashScreenLabelAnimation : UIAnimation
    {
        [SerializeField]
        private UICanvas m_idleLabel;
        [SerializeField]
        [HideInInspector]
        private UIAnimation[] m_idleAnimations;
        [SerializeField]
        private UICanvas m_transistionLabel;
        [SerializeField]
        [HideInInspector]
        private UIAnimation[] m_transistionAnimations;

        public override void Pause()
        {
            for (int i = 0; i < (m_idleAnimations?.Length ?? 0); i++)
            {
                m_idleAnimations[i].Pause();
            }

            for (int i = 0; i < (m_transistionAnimations?.Length ?? 0); i++)
            {
                m_transistionAnimations[i].Pause();
            }
        }

        public override void Play()
        {
            m_idleLabel.Hide();
            for (int i = 0; i < (m_idleAnimations?.Length ?? 0); i++)
            {
                m_idleAnimations[i].Stop();
            }

            m_transistionLabel.Show();
            for (int i = 0; i < (m_transistionAnimations?.Length ?? 0); i++)
            {
                m_transistionAnimations[i].Play();
            }
        }

        public override void Stop()
        {
            if (m_idleLabel)
            {
                m_idleLabel.Show();
            }
            for (int i = 0; i < (m_idleAnimations?.Length ?? 0); i++)
            {
                m_idleAnimations[i].Play();
            }

            if (m_transistionLabel)
            {
                m_transistionLabel.Hide();
            }
            for (int i = 0; i < (m_transistionAnimations?.Length ?? 0); i++)
            {
                m_transistionAnimations[i].Stop();
            }
        }

        protected override void Awake()
        {
            Stop();
            if (m_startOnAwake)
            {
                Play();
            }
        }

        private void OnValidate()
        {
            m_idleAnimations = m_idleLabel?.GetComponentsInChildren<UIAnimation>() ?? null;
            m_transistionAnimations = m_transistionLabel?.GetComponentsInChildren<UIAnimation>() ?? null;
            //Stop();
        }
    }
}