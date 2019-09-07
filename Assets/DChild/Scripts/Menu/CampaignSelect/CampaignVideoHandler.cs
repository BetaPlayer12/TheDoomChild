using Doozy.Engine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Menu.Campaign
{
    public class CampaignVideoHandler : MonoBehaviour
    {
        public enum Type
        {
            Intro,
            Next,
            Previous,
            First,
            Last,
        }

        [SerializeField]
        private VideoPlayer m_vidPlayer;
        [SerializeField, BoxGroup("Intro")]
        private VideoClip m_intro1;
        [SerializeField, BoxGroup("Intro")]
        private VideoClip m_intro2;
        [SerializeField]
        private VideoClip m_next;
        [SerializeField]
        private VideoClip m_previous;
        [SerializeField]
        private VideoClip m_first;
        [SerializeField]
        private VideoClip m_last;

        [Button]
        public void PlayIntro()
        {
            Play(Type.Intro);
        }

        public void Play(Type type)
        {
            switch (type)
            {
                case Type.Intro:
                    m_vidPlayer.loopPointReached += OnIntro1End;
                    PlayClip(m_intro1);
                    break;
                case Type.First:
                    m_vidPlayer.loopPointReached += OnEnd;
                    PlayClip(m_first);
                    break;
                case Type.Last:
                    m_vidPlayer.loopPointReached += OnEnd;
                    PlayClip(m_last);
                    break;
                case Type.Next:
                    m_vidPlayer.loopPointReached += OnEnd;
                    PlayClip(m_next);
                    break;
                case Type.Previous:
                    m_vidPlayer.loopPointReached += OnEnd;
                    PlayClip(m_previous);
                    break;
            }
        }

        private void OnEnd(VideoPlayer source)
        {
            GameEventMessage.SendEvent("Show Info");
            m_vidPlayer.loopPointReached -= OnIntro1End;
        }

        private void PlayClip(VideoClip clip)
        {
            m_vidPlayer.clip = clip;
            m_vidPlayer.Play();
        }

        private void OnIntro1End(VideoPlayer source)
        {
            PlayClip(m_intro2);
            m_vidPlayer.loopPointReached -= OnIntro1End;
        }
    } 
}
