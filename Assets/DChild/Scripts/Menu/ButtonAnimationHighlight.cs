using DChild.Menu.UI;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.MainMenu
{
    public class ButtonAnimationHighlight : UIHighlight
    {
        [SerializeField]
        private Image m_targetGraphic;
        [SerializeField]
        private CountdownTimer m_frameSpeed;
        [SerializeField, OnValueChanged("UpdateHighlight")]
        private bool m_highlight;

        [SerializeField, InlineEditor]
        private ButtonAnimationData m_animationData;
        private int m_frameIndex;

        public override void Highlight()
        {
            m_highlight = true;
            m_frameSpeed.Reset();
            enabled = true;
        }

        public override void Normalize()
        {
            m_highlight = false;
            m_frameSpeed.Reset();
            enabled = true;
        }

        public override void UseHighlightState()
        {
            m_highlight = true;
            m_frameIndex = m_animationData.frameCount - 1;
            UpdateTargetGraphic();
        }

        public override void UseNormalizeState()
        {
            m_highlight = false;
            m_frameIndex = 0;
            UpdateTargetGraphic();
        }

        private void ApplyColor()
        {
            if (m_targetGraphic.sprite == null)
            {
                m_targetGraphic.color = m_animationData.nullFrameColor;
            }
            else
            {
                m_targetGraphic.color = m_animationData.frameColor;
            }
        }

        private void UpdateTargetGraphic()
        {
            m_targetGraphic.sprite = m_animationData.GetFrame(m_frameIndex);
            m_targetGraphic.GraphicUpdateComplete();
            m_frameSpeed.Reset();
            ApplyColor();
        }

        private void OnFrameEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_highlight)
            {
                if (m_frameIndex == m_animationData.frameCount - 1)
                {
                    enabled = false;
                }
                else
                {
                    m_frameIndex++;
                    UpdateTargetGraphic();
                }
            }
            else
            {
                if (m_frameIndex == 0)
                {
                    enabled = false;
                }
                else
                {
                    m_frameIndex--;
                    UpdateTargetGraphic();
                }
            }
        }

        private void Start()
        {
            m_frameIndex = 0;
            m_frameSpeed.CountdownEnd += OnFrameEnd;
        }

        private void Update()
        {
            m_frameSpeed.Tick(Time.unscaledDeltaTime);
        }

#if UNITY_EDITOR
        private void UpdateHighlight()
        {
            if (m_animationData.frameCount > 0)
            {
                if (m_highlight)
                {
                    m_frameIndex = m_animationData.frameCount - 1;
                }
                else
                {
                    m_frameIndex = 0;
                }
                UpdateTargetGraphic();
                ApplyColor();
            }
        }
#endif
    }
}