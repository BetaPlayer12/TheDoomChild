using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay
{
    [SerializeField]
    public class PrimarySkillNotification : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnInspectorGUI("UpdateNotification")]
#endif
        private PrimarySkillData m_notifiedSkill;
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
            m_notifiedSkill = skill;
            m_border.sprite = skill.border;
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