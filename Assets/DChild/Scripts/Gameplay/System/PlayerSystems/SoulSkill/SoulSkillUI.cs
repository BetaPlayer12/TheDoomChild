using DChild.Gameplay.Characters.Players.SoulSkills;
using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillSelected : IEventActionArgs
    {
        public SoulSkillUI soulskillUI { get; private set; }
        public void Initialize(SoulSkillUI soulSkill)
        {
            this.soulskillUI = soulSkill;
        }
    }

    public abstract class SoulSkillUI : MonoBehaviour
    {
        [SerializeField]
        protected Image m_icon;

        protected Button m_button;
        protected bool m_isAnActivatedSoulSkill;

        public bool isAnActivatedSoulSkill => m_isAnActivatedSoulSkill;
        public Sprite soulSkillIcon => m_icon.sprite;
        public int soulSkillID { get; private set; }
        public event EventAction<SoulSkillSelected> OnHighlighted;
        public event EventAction<SoulSkillSelected> OnSelected;
        public event EventAction<SoulSkillSelected> OnClick;

        public void DisplayAs(SoulSkill soulSkill)
        {
            if (soulSkill == null)
            {
                soulSkillID = -1;
            }
            else
            {
                soulSkillID = soulSkill.id;
                m_icon.sprite = soulSkill.icon;
            }
        }

        public void CopyUI(SoulSkillUI reference)
        {
            m_isAnActivatedSoulSkill = reference.isAnActivatedSoulSkill;
            soulSkillID = reference.soulSkillID;
            m_icon.sprite = reference.soulSkillIcon;
        }

        public virtual void Show(bool immidiate)
        {
            m_button.interactable = true;
        }

        public virtual void Hide(bool immidiate)
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
