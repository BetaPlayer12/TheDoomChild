
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Holysoft.UI
{
    public class UITransistionTimeline : UITransistion
    {
        [SerializeField]
        private PlayableDirector m_timeline;

        public override void Pause()
        {
            m_timeline.Pause();
        }

        public override void Play()
        {
            m_timeline.Play();
        }

        public override void Stop()
        {
            m_timeline.Stop();
        }

        private void OnStopped(PlayableDirector obj)
        {
            CallTransistionEnd();
        }

        protected override void Awake()
        {
            m_timeline.stopped += OnStopped;
            base.Awake();
        }
    }
}