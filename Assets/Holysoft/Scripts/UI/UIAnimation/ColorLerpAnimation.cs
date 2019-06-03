using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class ColorLerpAnimation : UIAnimation
    {
        [SerializeField]
        [PropertyOrder(1)]
        protected LerpDuration m_lerp;
        [SerializeField]
        [PropertyOrder(1)]
        protected Color m_color1 = Color.white;
        [SerializeField]
        [PropertyOrder(1)]
        protected Color m_color2 = new Color(1, 1, 1, 0);
        protected bool m_toColor2;

        protected abstract Color target { set; }
        protected abstract void Validate();

        public override void Pause()
        {
            enabled = false;
        }

        public override void Play()
        {
            enabled = true;
        }

        public override void Stop()
        {
            enabled = false;
            m_lerp.SetValue(0);
            target = Color.Lerp(m_color1, m_color2, m_lerp.lerpValue);
            m_toColor2 = true;
        }

        public override void Enable()
        {
            base.Enable();
            Play();
        }

        public override void Disable()
        {
            base.Disable();
            Stop();
        }

        protected override void Awake()
        {
            base.Awake();
            target = m_color1;
            m_toColor2 = true;
        }

        private void Update()
        {
            if (m_toColor2)
            {
                m_lerp.Update(Time.unscaledDeltaTime);
                target = Color.Lerp(m_color1, m_color2, m_lerp.lerpValue);
                if (m_lerp.lerpValue == 1)
                {
                    if (m_repeat)
                    {
                        m_toColor2 = false;
                    }
                    else
                    {
                        //enabled = false;
                        CallAnimationEnd();
                    }
                    CallAnimationLoopReached();
                }
            }
            else
            {
                m_lerp.Update(-Time.unscaledDeltaTime);
                target = Color.Lerp(m_color1, m_color2, m_lerp.lerpValue);
                if (m_lerp.lerpValue == 0)
                {
                    m_toColor2 = true;
                    CallAnimationLoopReached();
                }
            }
        }

        private void OnValidate()
        {
            Validate();
        }
    }

}