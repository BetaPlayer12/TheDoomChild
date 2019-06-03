using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class ImageColorLerpAnimation : ColorLerpAnimation
    {
        [SerializeField]
        private Image m_target;

        protected override Color target { set => m_target.color = value; }

        protected override void Validate()
        {
            if (m_target == null)
            {
                m_target = GetComponent<Image>();
            }
            if (m_target != null)
            {
                m_target.color = m_color1;
            }
        }
    }

}