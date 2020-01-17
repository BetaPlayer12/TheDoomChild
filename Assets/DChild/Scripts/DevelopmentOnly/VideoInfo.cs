using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DChild
{
    [System.Serializable]
    public class VideoInfo
    {
        [SerializeField]
        private VideoClip m_clip;
        [SerializeField, MasterCustomEvent]
        private string m_event;


        public void Play(VideoPlayer player, Transform transform)
        {
            player.Stop();
            player.clip = m_clip;
            player.Play();
            MasterAudio.FireCustomEvent(m_event, transform);
        }
    }
}
