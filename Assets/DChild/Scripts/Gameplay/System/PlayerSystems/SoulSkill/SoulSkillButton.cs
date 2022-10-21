using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillSelected : IEventActionArgs
    {
        public SoulSkillButton soulskillUI { get; private set; }
        public void Initialize(SoulSkillButton soulSkill)
        {
            this.soulskillUI = soulSkill;
        }
    }

    public abstract class SoulSkillButton : SoulSkillUI
    {

        protected Button m_button;
        protected bool m_isAnActivatedSoulSkill;

        public bool isAnActivatedSoulSkill => m_isAnActivatedSoulSkill;

        public event EventAction<SoulSkillSelected> OnHighlighted;
        public event EventAction<SoulSkillSelected> OnSelected;
        public event EventAction<SoulSkillSelected> OnClick;


        public override void CopyUI(SoulSkillButton reference)
        {
            m_isAnActivatedSoulSkill = reference.isAnActivatedSoulSkill;
            base.CopyUI(reference);
        }

        public override void Show(bool immidiate)
        {
            m_button.interactable = true;
        }

        public override void Hide(bool immidiate)
        {
            m_button.interactable = false;
        }

        public virtual void SetIsAnActivatedUIState(bool isAnEquippedUI)
        {
            m_isAnActivatedSoulSkill = isAnEquippedUI;
        }

        public void Highlight()
        {
            SendEvent(OnHighlighted);
        }

        public void Select()
        {
            SendEvent(OnSelected);
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }

        public void Click()
        {
            SendEvent(OnClick);
        }

        private void SendEvent(EventAction<SoulSkillSelected> eventAction)
        {
            using (Cache<SoulSkillSelected> cacheEvent = Cache<SoulSkillSelected>.Claim())
            {
                cacheEvent.Value.Initialize(this);
                eventAction?.Invoke(this, cacheEvent.Value);
                cacheEvent.Release();
            }
        }

        protected virtual void Awake()
        {
            m_button = GetComponent<Button>();
        }
    }
}
