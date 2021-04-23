﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class ImageColorHighlight : UIHighlight
    {
        [SerializeField]
        private Image m_target;
        [SerializeField]
        private Color m_deselected = Color.white;
        [SerializeField]
        private Color m_selected = Color.red;

        public override void Normalize()
        {
            m_target.color = m_deselected;
        }

        public override void Highlight()
        {
            m_target.color = m_selected;
        }

        public override void UseHighlightState()
        {
            m_target.color = m_selected;

        }

        public override void UseNormalizeState()
        {
            m_target.color = m_deselected;
        }

        private void OnValidate()
        {
            if (m_target == null)
            {
                m_target = GetComponentInChildren<Image>();
            }
            if (m_target != null)
            {
                m_target.color = m_deselected;
            }
        }


    }
}