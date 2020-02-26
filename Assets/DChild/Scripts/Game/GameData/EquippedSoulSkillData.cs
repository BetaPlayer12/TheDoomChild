using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using DChild.Gameplay.Characters.Players;

namespace DChild.Serialization
{
    [System.Serializable]
    public struct EquippedSoulSkillData
    {
        [SerializeField, ValueDropdown("GetArmorSkills")]
        private int m_armorSkill;
        [SerializeField, ValueDropdown("GetSupportSkills")]
        private int m_supportSkill;
        [SerializeField, ValueDropdown("GetWeaponSkills")]
        private int m_weaponSkill1;
        [SerializeField, ValueDropdown("GetWeaponSkills")]
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

#if UNITY_EDITOR
        private IEnumerable GetArmorSkills()
        {
            var list = DChildUtility.GetSoulSkillsOfType(SoulSkillType.Armor);
            list.Insert(0, new ValueDropdownItem<int>("None", 0));
            return list;
        }

        private IEnumerable GetWeaponSkills()
        {
            var list = DChildUtility.GetSoulSkillsOfType(SoulSkillType.Weapon);
            list.Insert(0, new ValueDropdownItem<int>("None", 0));
            return list;
        }

        private IEnumerable GetSupportSkills()
        {
            var list = DChildUtility.GetSoulSkillsOfType(SoulSkillType.Support);
            list.Insert(0, new ValueDropdownItem<int>("None", 0));
            return list;
        }
#endif
    }

}