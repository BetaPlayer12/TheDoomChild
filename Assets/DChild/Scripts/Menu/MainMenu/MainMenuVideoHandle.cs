using DarkTonic.MasterAudio;
using Holysoft.Event;
using System;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Menu.MainMenu
{
    public class MainMenuVideoHandle : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_videoPlayer;
        [SerializeField]
        private VideoClip m_intro;
        [SerializeField]
        private VideoClip m_loop;
        [SerializeField]
        private VideoClip m_end;
        [SerializeField]
        private RenderTexture m_texture;
        [SerializeField]
        private Texture m_blank;
        [SerializeField]
        private EventSounds m_audio;

        private bool m_isPlayingIntroClip;
        public event EventAction<EventActionArgs> OnVideoEnd;

        public void ClearRenderer()
        {
            Graphics.Blit(m_blank, m_texture);
        }


        public void PlayIntro()
        {
            ClearRenderer();
            m_isPlayingIntroClip = true;
            PlayVideo(m_intro);
            m_videoPlayer.isLooping = false;
            m_audio.ActivateCodeTriggeredEvent1();
        }

        public void PlayEnd()
        {
            m_isPlayingIntroClip = false;
            PlayVideo(m_end);
            m_videoPlayer.isLooping = false;
            m_audio.ActivateCodeTriggeredEvent2();
        }

        private void OnLoopReach(VideoPlayer source)
        {
            if (m_isPlayingIntroClip)
            {
                PlayVideo(m_loop);
                m_videoPlayer.isLooping = true;
                m_isPlayingIntroClip = false;
            }
            else
            {
                OnVideoEnd?.Invoke(this,EventActionArgs.Empty);
            }
        }

        private void PlayVideo(VideoClip clip)
        {
            m_videoPlayer.clip = clip;
            m_videoPlayer.Play();
        }

        private void Awake()
        {
            m_videoPlayer.loopPointReached += OnLoopReach;
        }


    }
}