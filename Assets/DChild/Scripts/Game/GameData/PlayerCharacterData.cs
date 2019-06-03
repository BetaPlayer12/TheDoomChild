using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Sirenix.Utilities.Editor;

namespace DChild.Serialization
{
    [System.Serializable]
    public struct PlayerCharacterData
    {
        [SerializeField]
        private PlayerAttributeData m_attributeData;
        [SerializeField]
        private PlayerSkillsData m_skills;
        [Title("Soul Skills")]
        [SerializeField, Indent]
        private EquippedSoulSkillData m_equippedSoulSkillData;

        public PlayerAttributeData attributeData { get => m_attributeData; set => m_attributeData = value; }
        public PlayerSkillsData skills { get => m_skills; set => m_skills = value; }
        public EquippedSoulSkillData equippedSoulSkillData { get => m_equippedSoulSkillData; set => m_equippedSoulSkillData = value; }
    }
}