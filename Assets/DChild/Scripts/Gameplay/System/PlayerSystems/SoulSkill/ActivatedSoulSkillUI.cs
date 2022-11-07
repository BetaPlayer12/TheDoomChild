using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class ActivatedSoulSkillUI : SoulSkillButton
    {
        [SerializeField]
        private GameObject m_chain;

        public override void Show(bool immidiate)
        {
            if(m_button == null)
            {
                Awake();
            }
            m_button.interactable = true;
            m_chain.SetActive(true);
        }

        public override void Hide(bool immidiate)
        {
            if (m_button == null)
            {
                Awake();
            }
            m_button.interactable = false;
            m_chain.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            m_isAnActivatedSoulSkill = true;
        }
    }
}
