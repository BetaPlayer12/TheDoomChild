using System;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu.MainMenu
{

    public class MainMenuAnimation : UIElement3D
    {
        [SerializeField]
        private PlayableDirector m_introDirector;
        [SerializeField]
        private PlayableDirector m_idleDirector;
        [SerializeField]
        private bool m_startOnAwake;
        private bool m_hasPlayed;

        public override void Hide()
        {
            m_startOnAwake = false;
            if (m_hasPlayed)
            {
                m_introDirector.Stop();
                m_idleDirector.Stop();
            }
        }

        public override void Show()
        {
            m_startOnAwake = true;
            if (m_hasPlayed)
            {
                m_idleDirector.Play();
            }
            else
            {
                m_introDirector.Play();
            }
        }

        private void OnIntroStopped(PlayableDirector obj)
        {
            m_idleDirector.Play();
        }

        public void ResetAnimation()
        {
            m_hasPlayed = false;
        }

        private void OnIdleStopped(PlayableDirector obj)
        {
            m_idleDirector.Play();
        }

        private void Awake()
        {
            m_introDirector.stopped += OnIntroStopped;
            m_idleDirector.stopped += OnIdleStopped;
        }

        private void Start()
        {
            if (m_startOnAwake)
            {
                m_introDirector.Play();
                m_hasPlayed = true;
            }
        }
    }
}