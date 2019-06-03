using System;
using Holysoft.Event;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu
{
    public class CampaignSelectIntro : UIAnimation
    {
        [SerializeField]
        private PlayableDirector m_intro;
        [SerializeField]
        private CanvasGroupFadeAnimation m_fadeAnimation;

        public override void Play()
        {
            m_fadeAnimation.Stop();
            m_intro.Play();
        }

        public override void Pause()
        {
        }

        public override void Stop()
        {

        }

        private void OnStopped(PlayableDirector obj)
        {
            m_fadeAnimation.Play();
        }

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            CallAnimationEnd();
        }

        protected override void Awake()
        {
            base.Awake();
            m_intro.stopped += OnStopped;
            m_fadeAnimation.AnimationEnd += OnAnimationEnd;
        }
    }
}