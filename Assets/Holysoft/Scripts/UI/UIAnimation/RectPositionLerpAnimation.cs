using UnityEngine;

namespace Holysoft.UI
{
    public class RectPositionLerpAnimation : RectLerpAnimation
    {
        protected override Vector3 target { get => m_target.anchoredPosition3D; set => m_target.anchoredPosition3D = value; }

        public override void Pause()
        {
            enabled = false;
        }

        public override void Stop()
        {
            enabled = false;
            target = m_fromValue;
            m_lerp.SetValue(0);
        }

        private void Update()
        {
            m_lerp.Update(Time.deltaTime);
            target = Vector3.Lerp(m_fromValue, m_toValue, m_lerp.lerpValue);
            if (m_lerp.lerpValue == 1)
            {
                enabled = false;
                CallAnimationEnd();
            }
        }
    }

}