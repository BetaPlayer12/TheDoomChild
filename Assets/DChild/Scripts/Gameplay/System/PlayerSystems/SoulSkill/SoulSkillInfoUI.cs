﻿using DChild.Gameplay.Characters.Players.SoulSkills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillInfoUI : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_parentCanvas;
        [SerializeField]
        private SoulSkillUI m_skillUI;
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private TextMeshProUGUI m_capcity;
        [SerializeField]
        private TextMeshProUGUI m_description;


        public void DisplayInfoOf(SoulSkill soulSkill)
        {
            m_parentCanvas.enabled = soulSkill != null;
            m_skillUI.DisplayAs(soulSkill);
            m_name.text = soulSkill.name;
            m_capcity.text = soulSkill.capacity.ToString();
            m_description.text = soulSkill.description;
        }
    }
}
