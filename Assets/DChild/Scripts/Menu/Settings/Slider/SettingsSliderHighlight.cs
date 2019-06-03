using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.UI
{
    public class SettingsSliderHighlight : UIHighlight
    {
        [BoxGroup("References")]
        [SerializeField]
        private UICanvas m_markers;
        [BoxGroup("References")]
        [SerializeField]
        private UICanvas m_markerGlows;
        [BoxGroup("References")]
        [SerializeField]
        private Image m_handleSlideGlow;
        [BoxGroup("References")]
        [SerializeField]
        private Image m_handleGlow;

        public override void Highlight()
        {
            m_markers.Hide();
            m_markerGlows.Show();
            m_handleSlideGlow.enabled = true;
            m_handleGlow.enabled = true;
        }

        public override void Normalize()
        {
            m_markers.Show();
            m_markerGlows.Hide();
            m_handleSlideGlow.enabled = false;
            m_handleGlow.enabled = false;
        }

        public override void UseHighlightState()
        {
            Highlight();
        }

        public override void UseNormalizeState()
        {
            Normalize();
        }
    }
}