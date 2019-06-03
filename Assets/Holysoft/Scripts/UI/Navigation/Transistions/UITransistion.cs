using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class UITransistion : MonoBehaviour
    {
        [SerializeField]
        private bool m_startOnAwake;

        public event EventAction<EventActionArgs> TransistionEnd;
        public bool startOnAwake { set => m_startOnAwake = value; }

        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();

        protected void CallTransistionEnd()
        {
            TransistionEnd?.Invoke(this, EventActionArgs.Empty);
        }

        protected virtual void Awake()
        {
            if (m_startOnAwake)
            {
                Play();
            }
        }
    }
}