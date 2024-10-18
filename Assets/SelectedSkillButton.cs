using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle
{
    public class SelectedSkillButton : MonoBehaviour
    {
        [SerializeField]
        private Sprite m_meleeIcon;
        [SerializeField]
        private Sprite m_rangedIcon;
        [SerializeField]
        private Sprite m_magicIcon;

        [SerializeField]
        private Image m_targetIcon;

        public void Display(DamageType type)
        {
            //sets icon asset based on received DamageType
            switch (type)
            {
                case DamageType.Melee:
                    m_targetIcon.sprite = m_meleeIcon;
                    break;
                case DamageType.Range:
                    m_targetIcon.sprite = m_rangedIcon;
                    break;
                case DamageType.Magic:
                    m_targetIcon.sprite = m_magicIcon;
                    break;
            }
        }
    }
}