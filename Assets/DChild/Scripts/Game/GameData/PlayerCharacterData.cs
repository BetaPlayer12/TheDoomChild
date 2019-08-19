using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace DChild.Serialization
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        [SerializeField]
        private AcquisitionData m_bestiaryProgressData;
        [SerializeField]
        private PrimarySkillsData m_skills;
        [Title("Soul Skills")]
        [SerializeField, Indent]
        private AcquisitionData m_soulSkillAcquisitionData;
        [SerializeField, Indent]
        private EquippedSoulSkillData m_equippedSoulSkillData;

        public AcquisitionData bestiaryProgressData { get => m_bestiaryProgressData;}
        public PrimarySkillsData skills { get => m_skills; }
        public AcquisitionData soulSkillAcquisitionData { get => m_soulSkillAcquisitionData; }
        public EquippedSoulSkillData equippedSoulSkillData { get => m_equippedSoulSkillData;}
    }

    [System.Serializable]
    public struct PlayerInventoryData
    {
        [SerializeField]
        private int m_ID;
        [SerializeField, MinValue(0)]
        private int m_count;

        public PlayerInventoryData(int m_ID, int m_count)
        {
            this.m_ID = m_ID;
            this.m_count = m_count;
        }

        public int ID { get => m_ID;}
        public int count { get => m_count;}
    }
}