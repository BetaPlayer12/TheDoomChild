using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Characters.Players.SoulSkills;
using System;
using DChild.Gameplay.SoulSkills;

namespace DChild.Serialization
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        [SerializeField,TabGroup("Inventory"),HideReferenceObjectPicker,HideLabel]
        private PlayerInventoryData m_inventoryData;
        [SerializeField, TabGroup("Bestiary"),HideReferenceObjectPicker,HideLabel]
        private AcquisitionData m_bestiaryProgressData;
        [SerializeField, TabGroup("Skills")]
        private PrimarySkillsData m_skills;
        [SerializeField, TabGroup("Soul Skill")]
        private AcquisitionData m_soulSkillAcquisitionData;
        [SerializeField, TabGroup("Soul Skill")]
        private PlayerSoulSkillData m_soulSkillData;

        public PlayerInventoryData inventoryData => m_inventoryData;
        public AcquisitionData bestiaryProgressData { get => m_bestiaryProgressData; }
        public PrimarySkillsData skills { get => m_skills; }
        public AcquisitionData soulSkillAcquisitionData { get => m_soulSkillAcquisitionData; }
        public PlayerSoulSkillData soulSkillData { get => m_soulSkillData; }

        public PlayerCharacterData()
        {
            m_inventoryData = new PlayerInventoryData();
            m_bestiaryProgressData = new AcquisitionData();
            m_skills = new PrimarySkillsData();
            m_soulSkillAcquisitionData = new AcquisitionData();
            m_soulSkillData = new PlayerSoulSkillData();
        }

        public PlayerCharacterData(PlayerInventoryData m_inventoryData, AcquisitionData m_bestiaryProgressData, PrimarySkillsData m_skills,  AcquisitionData m_soulSkillAcquisitionData, PlayerSoulSkillData m_soulSkillData)
        {
            this.m_inventoryData = m_inventoryData;
            this.m_bestiaryProgressData = m_bestiaryProgressData;
            this.m_skills = m_skills;
            this.m_soulSkillAcquisitionData = m_soulSkillAcquisitionData;
            this.m_soulSkillData = m_soulSkillData;
        }

        public PlayerCharacterData(PlayerCharacterData data)
        {
            this.m_inventoryData = new PlayerInventoryData(data.inventoryData);
            this.m_bestiaryProgressData = new AcquisitionData(data.bestiaryProgressData);
            this.m_skills = data.skills;
            this.m_soulSkillAcquisitionData = new AcquisitionData(data.soulSkillAcquisitionData);
            this.m_soulSkillData = data.soulSkillData;
        }

        public void SetPrimarySkillData(PrimarySkillsData data)
        {
            m_skills = data;
        }
#if UNITY_EDITOR
        [NonSerialized, ShowInInspector, BoxGroup("Debug")]
        private BestiaryList m_bestiaryList;
        [NonSerialized, ShowInInspector, BoxGroup("Debug")]
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
}