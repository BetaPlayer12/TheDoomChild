using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Menu
{
    public class NavigationTimelines : MonoBehaviour
    {
        public enum Type
        {
            Next,
            Previous,
            FirstItem,
            LastItem
        }

        [SerializeField]
        private PlayableDirector m_prev;
        [SerializeField]
        private PlayableDirector m_next;
        [SerializeField]
        private PlayableDirector m_last;
        [SerializeField]
        private PlayableDirector m_first;

        public void Play(Type type)
        {
            GetTimelineFor(type)?.Play();
        }

        private PlayableDirector GetTimelineFor(Type type)
        {
            switch (type)
            {
                case Type.FirstItem:
                    return m_first;
                case Type.LastItem:
                    return m_last;
                case Type.Next:
                    return m_next;
                case Type.Previous:
                    return m_prev;
                default:
                    return null;
            }
        }
    }

}