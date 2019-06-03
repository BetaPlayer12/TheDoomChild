using Holysoft.Event;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChild.Menu.UI
{
    [ExecuteAlways]
    public sealed class ToggleStateLabel : ToggleVisual
    {

        [System.Serializable]
        public struct LabelSettings
        {
            [SerializeField]
            private string m_label;
            [SerializeField]
            private Color m_color;

            public LabelSettings(string m_label)
            {
                this.m_label = m_label;
                m_color = Color.white;
            }

            public string label => m_label;

            public Color color => m_color;
        }

        [SerializeField]
        private TextMeshProUGUI m_label;
        [BoxGroup("Label")]
        [SerializeField]
        private LabelSettings m_trueSettings = new LabelSettings("ON");
        [BoxGroup("Label")]
        [SerializeField]
        private LabelSettings m_falseSettings = new LabelSettings("OFF");

        protected override void OnStateUpdate(object sender, ButtonToggledEventArgs eventArgs)
        {
            ChangeTest(eventArgs.isTrue ? m_trueSettings : m_falseSettings);
        }

        private void ChangeTest(LabelSettings settings)
        {
            m_label.text = settings.label;
            m_label.color = settings.color;
        }
    }

}