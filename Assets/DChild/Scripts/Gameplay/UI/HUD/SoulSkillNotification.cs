using DChild.Gameplay.Characters.Players.SoulSkills;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay
{
    public class SoulSkillNotification : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnInspectorGUI("UpdateNotification")]
#endif
        private SoulSkill m_notifiedSkill;
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_skillName;
        [SerializeField]
        private TextMeshProUGUI m_description;

        public void SetNotifiedSkill(SoulSkill skill)
        {
            m_notifiedSkill = skill;
            m_icon.sprite = skill.icon;
            m_skillName.text = skill.name;
            m_description.text = skill.description;
        }

#if UNITY_EDITOR
        private void UpdateNotification()
        {
            if (Application.isPlaying)
            {
                if (m_notifiedSkill == null)
                {
                    m_icon.sprite = null;
                    m_skillName.text = "No Skill";
                    m_description.text = "No Description";
                }
                else
                {
                    m_icon.sprite = m_notifiedSkill.icon;
                    m_skillName.text = m_notifiedSkill.name;
                    m_description.text = m_notifiedSkill.description;
                }
            }
        }
#endif
    }
}