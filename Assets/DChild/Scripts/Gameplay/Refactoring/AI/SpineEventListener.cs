using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
{
    public class SpineEventListener : MonoBehaviour
    {
        public class EventAction
        {
            public event Action action;
            public void Invoke() => action?.Invoke();
        }

        [SerializeField]
        private SkeletonAnimation m_animation;

        private Dictionary<string, EventAction> m_listerners;
        private bool m_activated;

        public void Subscribe(string eventName, Action action)
        {
            if (m_listerners.ContainsKey(eventName) == false)
            {
                var eventAction = new EventAction();
                eventAction.action += action;
                m_listerners.Add(eventName, eventAction);
            }
            else
            {
                m_listerners[eventName].action += action;
            }
        }

        public void Unsubcribe(string eventName, Action action)
        {
            if (m_listerners.ContainsKey(eventName))
            {
                m_listerners[eventName].action -= action;
            }
        }

        public void Activate()
        {
            if (m_activated == false)
            {
                m_animation.AnimationState.Event += OnEvent;
                m_activated = true;
            }
        }

        private void OnEvent(TrackEntry trackEntry, Spine.Event e)
        {
            var eventName = e.Data.Name;
            if (m_listerners.ContainsKey(eventName))
            {
                m_listerners[eventName].Invoke();
            }
        }

        private void Awake()
        {
            m_listerners = new Dictionary<string, EventAction>();
        }

        private void Start()
        {
            Activate();
        }
    }
}