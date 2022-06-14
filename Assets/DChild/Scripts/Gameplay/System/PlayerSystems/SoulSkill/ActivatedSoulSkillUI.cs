using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class ActivatedSoulSkillUI : SoulSkillUI
    {
        [SerializeField]
        private GameObject m_chain;

        public override void Show(bool immidiate)
        {
            gameObject.SetActive(true);
            m_chain.SetActive(true);
        }

        public override void Hide(bool immidiate)
        {
            gameObject.SetActive(false);
            m_chain.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            m_isAnActivatedSoulSkill = true;
        }
    }
}
