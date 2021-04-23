using Holysoft.Collections;
using UnityEngine;

namespace Holysoft.UI
{
    public class RectScaleHighlight : UIHighlight
    {
        [SerializeField]
        private RectTransform m_target;
        [SerializeField]
        private RectScaleHighlightData m_data;
        private LerpDuration m_lerp;
        private bool m_isHighlighted;

        public override void Highlight()
        {
            enabled = true;
            m_isHighlighted = true;
        }

        public override void Normalize()
        {
            enabled = true;
            m_isHighlighted = false;
            m_target.anchoredPosition3D = m_data.deselected;
        }

        public override void UseHighlightState()
        {
            enabled = false;
            m_isHighlighted = true;
            m_target.anchoredPosition3D = m_data.selected;
            m_target.localScale = Vector3.Lerp(m_data.deselected, m_data.selected, 1);
        }

        public override void UseNormalizeState()
        {
            enabled = false;
            m_isHighlighted = false;
            m_target.localScale = Vector3.Lerp(m_data.deselected, m_data.selected, 0);
        }

        private void Awake()
        {
            m_isHighlighted = false;
            m_lerp = new LerpDuration(m_data.lerpDuration);
            m_lerp.SetValue(0);
            enabled = false;
        }

        private void LateUpdate()
        {
            if (m_isHighlighted)
            {
                m_lerp.Update(Time.unscaledDeltaTime);
                m_target.localScale = Vector3.Lerp(m_data.deselected, m_data.selected, m_lerp.lerpValue);
                if (m_lerp.lerpValue == 1)
                {
                    enabled = false;
                }
            }
            else
            {
                m_lerp.Update(-Time.unscaledDeltaTime);
                m_target.localScale = Vector3.Lerp(m_data.deselected, m_data.selected, m_lerp.lerpValue);
                if (m_lerp.duration == 0)
                {
                    enabled = false;
                }
            }
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_target);

            if (m_target != null && m_data != null)
            {
                m_target.localScale = m_data.deselected;
            }
        }
    }
}