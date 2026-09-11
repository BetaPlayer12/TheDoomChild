using DChild.Gameplay.ArmyBattle.SpecialSkills;
using Doozy.Runtime.UIManager.Components;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class SpecialSkillGroupOptionUI : AttackingGroupOptionUI
    {
        [SerializeField]
        private ArmyBattleSpecialSkillSelection m_skillSelection;
        [SerializeField]
        private Localize m_description;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private Sprite m_specialGlow;
        [SerializeField]
        private UIButton m_armyRow;
        [SerializeField]
        private GameObject m_usedOverlay;

        private ISpecialSkillGroup m_group;
        private bool m_isUsed;
        public ISpecialSkillGroup group => m_group;
        public bool isUsed => m_isUsed;

        public void SetUsed(bool isUsed)
        {
            m_isUsed = isUsed;

            if (m_armyRow == null)
            {
                m_armyRow = GetComponent<UIButton>();
            }

            if (m_usedOverlay == null)
            {
                var childTransforms = GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < childTransforms.Length; i++)
                {
                    if (childTransforms[i].name == "Image - UsedGroupOverlay")
                    {
                        m_usedOverlay = childTransforms[i].gameObject;
                        break;
                    }
                }
            }

            if (m_usedOverlay != null)
            {
                m_usedOverlay.SetActive(isUsed);
            }

            if (m_armyRow != null)
            {
                m_armyRow.interactable = !isUsed;
            }
        }

        public void Display(ISpecialSkillGroup group)
        {
            m_group = group;

            if (group != null)
            {
                selectedSkill.DisplaySpecialIcon();
                characterGroupUI.Display(group.GetCharacterGroup() ?? null);
                partyName.Display(group);
                gameObject.SetActive(true);

                var groupId = group.id.ToString("000");

                m_description.SetTerm($"ArmyBattle/Groups/{groupId}/AG_{groupId}_SpecialSkill");
                m_icon.sprite = group.GetSpecialSkill().icon;

                m_icon.color = m_icon.sprite ? Color.white : Color.clear;
                return;
            }

            m_isUsed = false;
            gameObject.SetActive(false);
        }
    }
}
