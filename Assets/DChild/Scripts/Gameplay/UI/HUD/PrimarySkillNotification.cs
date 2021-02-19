using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay
{
    [SerializeField]
    public class PrimarySkillNotification : MonoBehaviour
    {
        [SerializeField]
        private PrimarySkillList m_notifiedSkill;
        [SerializeField]
        private Image m_border;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_skillName;
        [SerializeField]
        private TextMeshProUGUI m_description;

        public void SetNotifiedSkill(PrimarySkillData skill)
        {
            m_border.sprite = skill.border;
            m_icon.sprite = skill.icon;
            m_skillName.text = skill.skillName;
            m_description.text = skill.description;
        }

        private void OnSkillUpdate(object sender, PrimarySkillUpdateEventArgs eventArgs)
        {
            if (eventArgs.isEnabled)
            {
                var skill = eventArgs.skill;
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

        private void Start()
        {
            GameplaySystem.playerManager.player.skills.SkillUpdate += OnSkillUpdate;
        }

    }
}