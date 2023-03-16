using DChild.Gameplay.Characters.Players.SoulSkills;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class AvailableSoulSkillUI : SoulSkillButton
    {
        [SerializeField]
        private GameObject m_shownVersion;
        [SerializeField]
        private GameObject m_hiddenVersion;

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
            if (m_button == null)
            {
                Awake();
            }

            m_hiddenVersion.SetActive(false);
            m_button.interactable = !isAnEquippedUI;
            base.SetIsAnActivatedUIState(isAnEquippedUI);
        }

        protected override void SetOrb(SoulSkillOrbData orbData)
        {
            SetOrb(orbData.availableOrb);
        }
    }
}
