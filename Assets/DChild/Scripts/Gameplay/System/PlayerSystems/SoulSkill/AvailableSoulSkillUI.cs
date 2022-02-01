using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class AvailableSoulSkillUI : SoulSkillUI
    {
        [SerializeField]
        private GameObject m_shownVersion;
        [SerializeField]
        private GameObject m_hiddenVersion;

        [SerializeField]
        private Button m_button;
        [SerializeField]
        private Color m_unactivatedColor;
        [SerializeField]
        private Color m_activatedColor;

        public override void Show(bool immidiate)
        {
            m_shownVersion.SetActive(true);
            m_hiddenVersion.SetActive(false);
        }

        public override void Hide(bool immidiate)
        {
            m_shownVersion.SetActive(false);
            m_hiddenVersion.SetActive(true);
        }

        public override void SetIsAnActivatedUIState(bool isAnEquippedUI)
        {
            base.SetIsAnActivatedUIState(isAnEquippedUI);
            var colors = m_button.colors;
            colors.normalColor = m_isAnActivatedSoulSkill ? m_activatedColor : m_unactivatedColor;
            m_button.colors = colors;
        }
    }
}
