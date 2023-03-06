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


        private void UpdateNotification(object sender, SoulSkillAcquiredEventArgs eventArgs)
        {
            SoulSkill skill = eventArgs.SoulSKill;
            SetNotifiedSkill(skill);
            m_icon.sprite = m_notifiedSkill.icon;
            m_skillName.text = m_notifiedSkill.name;
            m_description.text = m_notifiedSkill.description;
             
        }


        private void Start()
        {
            GameplaySystem.playerManager.player.inventory.SoulSkillItemAcquired += UpdateNotification;
        }
    }
}