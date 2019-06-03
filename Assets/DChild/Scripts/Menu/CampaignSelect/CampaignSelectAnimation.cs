using System;
using Holysoft.Event;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu
{
    public class CampaignSelectAnimation : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroupFadeAnimation m_fadeAnimation;
        [SerializeField]
        private PlayableDirector m_prev;
        [SerializeField]
        private PlayableDirector m_next;
        [SerializeField]
        private PlayableDirector m_last;
        [SerializeField]
        private PlayableDirector m_first;

        public event EventAction<EventActionArgs> InfoChangeRequest;
        public event EventAction<EventActionArgs> AnimationEnd;

        public void PlayPrevious()
        {
            HidePanel();
            m_prev.Play();
        }

        public void PlayNext()
        {
            HidePanel();
            m_next.Play();
        }

        public void PlayLast()
        {
            HidePanel();
            m_last.Play();
        }

        public void PlayFirst()
        {
            HidePanel();
            m_first.Play();
        }

        private void HidePanel()
        {
            m_fadeAnimation.Stop();
            InfoChangeRequest?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnStopped(PlayableDirector obj)
        {
            m_fadeAnimation.Play();
        }

        private void OnCanvasFadeIn(object sender, EventActionArgs eventArgs)
        {
            AnimationEnd?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_fadeAnimation.FadeInComplete += OnCanvasFadeIn;
            m_prev.stopped += OnStopped;
            m_next.stopped += OnStopped;
            m_last.stopped += OnStopped;
            m_first.stopped += OnStopped;
        }


    }
}