using DChild.Gameplay.Characters.Players;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.UI.PrimarySkills
{
    public class PrimarySkillSelectable : MonoBehaviour
    {
        [SerializeField]
        private PrimarySkillIcon m_icon;

        private PrimarySkillData m_reference;
        private UIToggle m_toggle;

        public PrimarySkillData reference => m_reference;

        public void SetAsUnlocked(bool isUnlocked)
        {
            m_toggle.interactable = isUnlocked;

            if(isUnlocked == true)
            {
                m_icon.ShowIcon();
            }
            else
            {
                m_icon.HideIcon();
            }
        }

        public void SetSelectableFor(PrimarySkillData data)
        {
            m_icon.DisplayAs(data);
            m_reference = data;
        }

        private void Awake()
        {
            m_toggle = GetComponent<UIToggle>();
        }
    }
}