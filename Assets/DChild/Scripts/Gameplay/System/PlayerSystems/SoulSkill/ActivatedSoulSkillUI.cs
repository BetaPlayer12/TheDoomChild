namespace DChild.Gameplay.SoulSkills.UI
{
    public sealed class ActivatedSoulSkillUI : SoulSkillUI
    {

        public override void Show(bool immidiate)
        {
            gameObject.SetActive(true);
        }

        public override void Hide(bool immidiate)
        {
            gameObject.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            m_isAnActivatedSoulSkill = true;
        }
    }
}
