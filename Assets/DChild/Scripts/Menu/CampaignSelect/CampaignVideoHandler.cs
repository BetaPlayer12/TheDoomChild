using DChild.Temp;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DarkTonic.MasterAudio;
using Doozy.Runtime.Signals;

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
        private VideoInfo m_intro1;
        [SerializeField, BoxGroup("Intro")]
        private VideoInfo m_intro2;
        [SerializeField, BoxGroup("Intro")]
        private SignalSender m_introEndSignal;
        [SerializeField]
        private VideoInfo m_next;
        [SerializeField]
        private VideoInfo m_previous;
        [SerializeField]
        private VideoInfo m_first;
        [SerializeField]
        private VideoInfo m_last;
        [SerializeField]
        private SignalSender m_transistionSignal;

        [SerializeField]
        private RenderTexture m_texture;
        [SerializeField]
        private Texture m_blank;

        public void ClearRenderer()
        {
            Graphics.Blit(m_blank, m_texture);
        }

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
                    m_intro1.Play(m_vidPlayer, transform);
                    break;
                case Type.First:
                    m_vidPlayer.loopPointReached += OnTransistionEnd;
                    m_first.Play(m_vidPlayer, transform);
                    break;
                case Type.Last:
                    m_vidPlayer.loopPointReached += OnTransistionEnd;
                    m_last.Play(m_vidPlayer, transform);
                    break;
                case Type.Next:
                    m_vidPlayer.loopPointReached += OnTransistionEnd;
                    m_next.Play(m_vidPlayer, transform);
                    break;
                case Type.Previous:
                    m_vidPlayer.loopPointReached += OnTransistionEnd;
                    m_previous.Play(m_vidPlayer, transform);
                    break;
            }
        }

        private void OnTransistionEnd(VideoPlayer source)
        {
            m_transistionSignal.SendSignal();
            m_vidPlayer.loopPointReached -= OnTransistionEnd;

        }

        private void OnIntro1End(VideoPlayer source)
        {
            m_intro2.Play(source, transform);
            m_vidPlayer.loopPointReached -= OnIntro1End;
            m_vidPlayer.loopPointReached += OnIntro2End;
        }

        private void OnIntro2End(VideoPlayer source)
        {
            m_introEndSignal.SendSignal();
            m_vidPlayer.loopPointReached -= OnIntro2End;
        }
    }
}
