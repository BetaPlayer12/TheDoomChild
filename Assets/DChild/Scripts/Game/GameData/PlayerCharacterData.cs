using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Characters.Players.SoulSkills;
using System;

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

        public AcquisitionData bestiaryProgressData { get => m_bestiaryProgressData; }
        public PrimarySkillsData skills { get => m_skills; }
        public AcquisitionData soulSkillAcquisitionData { get => m_soulSkillAcquisitionData; }
        public EquippedSoulSkillData equippedSoulSkillData { get => m_equippedSoulSkillData; }

        public PlayerCharacterData(AcquisitionData m_bestiaryProgressData, PrimarySkillsData m_skills, AcquisitionData m_soulSkillAcquisitionData, EquippedSoulSkillData m_equippedSoulSkillData)
        {
            this.m_bestiaryProgressData = m_bestiaryProgressData;
            this.m_skills = m_skills;
            this.m_soulSkillAcquisitionData = m_soulSkillAcquisitionData;
            this.m_equippedSoulSkillData = m_equippedSoulSkillData;
        }

        public PlayerCharacterData()
        {
            m_bestiaryProgressData = new AcquisitionData();
            m_skills = new PrimarySkillsData();
            m_soulSkillAcquisitionData = new AcquisitionData();
            m_equippedSoulSkillData = new EquippedSoulSkillData();
        }

#if UNITY_EDITOR
        [NonSerialized,ShowInInspector, BoxGroup("Debug")]
        private BestiaryList m_bestiaryList;
        [NonSerialized,ShowInInspector, BoxGroup("Debug")]
        private SoulSkillList m_soulSkillList;


        [Button, BoxGroup("Debug")]
        private void Initialize()
        {
            if (m_bestiaryList)
            {
                InitializeAcquisitionData(ref m_bestiaryProgressData, m_bestiaryList.GetIDs());
            }
            if (m_soulSkillList)
            {
                InitializeAcquisitionData(ref m_soulSkillAcquisitionData, m_soulSkillList.GetIDs());
            }
        }


        private void InitializeAcquisitionData(ref AcquisitionData acquisitionData, int[] IDS)
        {
            List<AcquisitionData.SerializeData> data = new List<AcquisitionData.SerializeData>();
            for (int i = 0; i < IDS.Length; i++)
            {
                data.Add(new AcquisitionData.SerializeData(IDS[i], false));
            }
            acquisitionData = new AcquisitionData(data.ToArray());
        }
#endif
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

        public int ID { get => m_ID; }
        public int count { get => m_count; }
    }
}