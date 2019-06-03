using UnityEngine;

namespace Holysoft.UI
{
    public class RectScalePunchAnimation : RectLerpAnimation
    {
        protected override Vector3 target { get => m_target.localScale; set => m_target.localScale = value; }

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
            target = Vector3.Lerp(m_fromValue, m_toValue, m_lerp.lerpValue);
            m_lerpToValue = true;
        }

        private void Update()
        {
            if (m_lerpToValue)
            {
                m_lerp.Update(Time.deltaTime);
                target = Vector3.Lerp(m_fromValue, m_toValue, m_lerp.lerpValue);
                if (m_lerp.lerpValue == 1)
                {
                    m_lerpToValue = false;
                }
            }
            else
            {
                m_lerp.Update(-Time.deltaTime);
                target = Vector3.Lerp(m_fromValue, m_toValue, m_lerp.lerpValue);
                if (m_lerp.lerpValue == 0)
                {
                    if (m_repeat)
                    {
                        m_lerpToValue = true;
                    }
                    else
                    {
                        enabled = false;
                        this.gameObject.SetActive(false);
                        CallAnimationEnd();
                    }
                    CallAnimationLoopReached();
                }
            }
        }
    }
}