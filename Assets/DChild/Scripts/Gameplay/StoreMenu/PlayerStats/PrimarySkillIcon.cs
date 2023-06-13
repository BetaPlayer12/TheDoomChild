using DChild.Gameplay.Characters.Players;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI.PrimarySkills
{
    public class PrimarySkillIcon : MonoBehaviour
    {
        [SerializeField]
        private Image m_border;
        [SerializeField]
        private Image m_icon;

        public void DisplayAs(PrimarySkillData skill)
        {
            m_border.sprite = skill.border;
            m_icon.sprite = skill.icon;
        }
    }
}