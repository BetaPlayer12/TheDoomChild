using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{

    public abstract class UIAnimation : UIBehaviour
    {
        [SerializeField]
        [PropertyOrder(100)]
        protected bool m_repeat = true;
        [SerializeField]
        [PropertyOrder(100)]
        protected bool m_startOnAwake = true;

        public event EventAction<EventActionArgs> AnimationLoopReached;
        public event EventAction<EventActionArgs> AnimationEnd;

        public abstract void Play();
        public virtual void Pause()
        {
            enabled = false;
        }

        public virtual void Resume()
        {
            enabled = false;
        }
        public abstract void Stop();

        protected void CallAnimationLoopReached() => AnimationLoopReached?.Invoke(this, EventActionArgs.Empty);
        protected void CallAnimationEnd()
        {
            AnimationEnd?.Invoke(this, EventActionArgs.Empty);
            //this.gameObject.SetActive(false);
        }

        protected virtual void Awake()
        {
            if (m_startOnAwake)
            {
                Play();
            }
            else
            {
                enabled = false;
            }
        }
    }
}