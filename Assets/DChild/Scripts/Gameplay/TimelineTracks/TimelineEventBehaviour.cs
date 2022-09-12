using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace DChild.Gameplay.Cinematics
{
    [System.Serializable]
    public class TimelineEventBehaviour : PlayableBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, TextArea]
        private string m_description;
#endif
        [SerializeField]
        private UnityEvent m_event;

        public UnityEvent eventToCall => m_event;
    }
}

