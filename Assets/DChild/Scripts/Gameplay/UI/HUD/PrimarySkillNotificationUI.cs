using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.UI.PrimarySkills;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay
{
    public class PrimarySkillNotificationUI : NotificationUI
    {
        [SerializeField]
        private PrimarySkillList m_notifiedSkill;
        [SerializeField]
        private PrimarySkillIcon m_icon;
        [SerializeField]
        private TextMeshProUGUI m_skillName;
        [SerializeField]
        private TextMeshProUGUI m_description;
        [SerializeField]
        private TextMeshProUGUI m_instruction;

        private const string INSTRUCTION_HEADER = "<color=#710B0D>Button:</color><indent=15%>";

        public void SetNotifiedSkill(PrimarySkillData skill)
        {
            m_icon.DisplayAs(skill);
            m_skillName.text = skill.skillName;
            m_description.text = skill.description;
            m_instruction.text = INSTRUCTION_HEADER + skill.instruction;
        }

        public void SetNotifiedSkill(PrimarySkill skill)
        {
            for (int i = 0; i < m_notifiedSkill.Count; i++)
            {
                var skillData = m_notifiedSkill.GetData(i);
                if (skillData.skill == skill)
                {
                    SetNotifiedSkill(skillData);
                    break;
                }
            }
        }
    }
}