using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DChild.Gameplay.Characters.Players;

namespace DChild.Serialization
{
    [System.Serializable]
    public struct EquippedSoulSkillData
    {
        [SerializeField]
        private int m_armorSkill;
        [SerializeField]
        private int m_supportSkill;
        [SerializeField]
        private int m_weaponSkill1;
        [SerializeField]
        private int m_weaponSkill2;

        public EquippedSoulSkillData(int m_armorSkill, int m_supportSkill, int m_weaponSkill1, int m_weaponSkill2)
        {
            this.m_armorSkill = m_armorSkill;
            this.m_supportSkill = m_supportSkill;
            this.m_weaponSkill1 = m_weaponSkill1;
            this.m_weaponSkill2 = m_weaponSkill2;
        }

        public int armorSkill => m_armorSkill;
        public int supportSkill => m_supportSkill;
        public int weaponSkill1 => m_weaponSkill1;
        public int weaponSkill2 => m_weaponSkill2;
    }

}