using TMPro;
using UnityEngine;

namespace Holysoft.UI
{
    public class LabelColorLerpAnimation : ColorLerpAnimation
    {
        [SerializeField]
        private TextMeshProUGUI m_target;

        protected override Color target { set => m_target.color = value; }

        protected override void Validate()
        {
            if (m_target == null)
            {
                m_target = GetComponent<TextMeshProUGUI>();
            }
            if (m_target != null)
            {
                m_target.color = m_color1;
            }
        }
    }

}