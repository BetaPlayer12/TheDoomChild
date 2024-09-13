using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace DChild.Gameplay.Cinematics
{
    public class CinematicVideoPlayer : MonoBehaviour
    {
        [System.Serializable]
        public class DelayEvent
        {
            [SerializeField]
            private float m_delay;
            [SerializeField]
            private UnityEvent m_events;

            public float delay => m_delay;
            public UnityEvent events => m_events;
        }

        [SerializeField]
        private VideoClip m_clip;
        [SerializeField, TabGroup("During")]
        private DelayEvent[] m_duringCinematicEvents;
        [SerializeField, TabGroup("After")]
        private UnityEvent m_afterCinematicEvent;

        [Button, HideInEditorMode]
        public void Play()
        {
            GameplaySystem.gamplayUIHandle.ShowCinematicVideo(m_clip, DuringCinematicRoutine, OnVideoDone);
        }

        private IEnumerator DuringCinematicRoutine()
        {
            if (m_duringCinematicEvents.Length == 0)
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < m_duringCinematicEvents.Length; i++)
                {
                    var eventInstance = m_duringCinematicEvents[i];
                    yield return new WaitForSeconds(eventInstance.delay);
                    eventInstance.events.Invoke();
                }
            }
        }

        private void OnVideoDone()
        {
            m_afterCinematicEvent.Invoke();
        }
    }
}