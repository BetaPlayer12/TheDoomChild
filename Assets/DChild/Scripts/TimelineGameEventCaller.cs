using DChild.Temp;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild
{
    public class TimelineGameEventCaller : MonoBehaviour
    {
        [SerializeField]
        private string m_event;

        [SerializeField]
        private PlayableDirector[] m_timelines;

        private void Awake()
        {
            for (int i = 0; i < m_timelines.Length; i++)
            {
                m_timelines[i].stopped += OnStopped;
            }
        }

        private void OnStopped(PlayableDirector obj)
        {
            GameEventMessage.SendEvent(m_event);
        }
    }

}