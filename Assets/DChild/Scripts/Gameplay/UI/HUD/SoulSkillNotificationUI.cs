using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.SoulSkills;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay
{
    public class SoulSkillNotificationUI : NotificationUI
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_skillName;
        [SerializeField]
        private TextMeshProUGUI m_description;

        public void SetNotifiedSkill(SoulSkill skill)
        {
            m_icon.sprite = skill.icon;
            m_skillName.text = skill.name;
            m_description.text = skill.description;
        }
    }
}