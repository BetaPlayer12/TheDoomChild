using Doozy.Runtime.Signals;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace DChild.Gameplay.Systems
{
    public class CinematicVideoHandle : MonoBehaviour
    {
        [SerializeField]
        private SignalSender m_videoCinemaStartSignal;
        [SerializeField]
        private SignalSender m_videoCinemaEndSignal;
        [SerializeField]
        private VideoPlayer m_videoPlayer;
        [SerializeField, Min(0)]
        private float m_fadeBufferTime = 1;

        private bool m_isPlaying;
        private bool m_videoClipPlaying;
        private Func<IEnumerator> m_behindTheSceneRoutine;
        private Action OnVideoDone;

        private Coroutine m_videoPlayingRoutine;

        public void ShowCinematicVideo(VideoClip clip, Func<IEnumerator> behindTheSceneRoutine = null, Action OnVideoDone = null)
        {
            if (m_isPlaying == false)
            {
                if(clip == null) {
                    Debug.LogWarning("There was an attempt to play a null video cinematic");

                    return;
                }

                m_videoPlayer.clip = clip;
                m_behindTheSceneRoutine = behindTheSceneRoutine;
                this.OnVideoDone = OnVideoDone;

                m_videoPlayingRoutine = StartCoroutine(VideoPlayingRoutine());
            }
            else
            {
                Debug.LogWarning("Attempting to Play a Video Clip while theres already a video clip being played");
            }
        }

        public void Initialize()
        {
            m_videoPlayer.loopPointReached += OnVideoClipDone;
        }

        private void OnVideoClipDone(VideoPlayer source)
        {
            m_videoClipPlaying = false;
            OnVideoDone?.Invoke();
        }

        private IEnumerator VideoPlayingRoutine()
        {
            var waitForFade = new WaitForSeconds(m_fadeBufferTime);

            m_isPlaying = true;
            GameplaySystem.gamplayUIHandle.ToggleFadeUI(true);
            yield return waitForFade;
            m_videoCinemaStartSignal?.SendSignal();

            m_videoClipPlaying = true;
            m_videoPlayer.Play();

            if (m_behindTheSceneRoutine != null)
            {
                yield return m_behindTheSceneRoutine();
            }
            while (m_videoClipPlaying)
                yield return null;

            m_videoPlayer.Stop();
            m_videoCinemaEndSignal?.SendSignal();
            OnVideoDone?.Invoke();
            yield return waitForFade;

            GameplaySystem.gamplayUIHandle.ToggleFadeUI(false);

            m_isPlaying = false;
            m_videoPlayingRoutine = null;
        }
    }
}
