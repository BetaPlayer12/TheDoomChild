using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Characters.Players.SoulSkills;
using System;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Leveling;
using DChild.Gameplay.Characters.Players;

namespace DChild.Serialization
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        [SerializeField]
        private PlayerLevel.SaveData m_levelData;
        [SerializeField, TabGroup("Inventory"), HideReferenceObjectPicker, HideLabel]
        private TradableInventorySerialization m_inventoryData;
        [SerializeField, TabGroup("Bestiary"), HideReferenceObjectPicker, HideLabel]
        private AcquisitionData m_bestiaryProgressData;
        [SerializeField, TabGroup("Skills")]
        private PrimarySkillsData m_skills;
        [SerializeField, TabGroup("Soul Skill")]
        private PlayerSoulSkillData m_soulSkillData;
        [SerializeField]
        private CombatArtsSaveData m_combatArtsData;

        public PlayerLevel.SaveData levelData => m_levelData;
        public TradableInventorySerialization inventoryData => m_inventoryData;
        public AcquisitionData bestiaryProgressData { get => m_bestiaryProgressData; }
        public PrimarySkillsData skills { get => m_skills; }
        public PlayerSoulSkillData soulSkillData { get => m_soulSkillData; }
        public CombatArtsSaveData combatArtsData { get => m_combatArtsData; }

        public PlayerCharacterData()
        {
            m_levelData = new PlayerLevel.SaveData();
            m_inventoryData = new TradableInventorySerialization();
            m_bestiaryProgressData = new AcquisitionData();
            m_skills = new PrimarySkillsData();
            m_soulSkillData = new PlayerSoulSkillData();
            m_combatArtsData = new CombatArtsSaveData(0, new int[0]);
        }

        public PlayerCharacterData(PlayerLevel.SaveData levelData, TradableInventorySerialization m_inventoryData, AcquisitionData m_bestiaryProgressData, PrimarySkillsData m_skills, PlayerSoulSkillData m_soulSkillData, CombatArtsSaveData combatArtsData)
        {
            m_levelData = levelData;
            this.m_inventoryData = m_inventoryData;
            this.m_bestiaryProgressData = m_bestiaryProgressData;
            this.m_skills = m_skills;
            this.m_soulSkillData = m_soulSkillData;
            m_combatArtsData = combatArtsData;
        }

        public PlayerCharacterData(PlayerCharacterData data)
        {
            m_levelData = data.levelData;
            this.m_inventoryData = new TradableInventorySerialization(data.inventoryData);
            this.m_bestiaryProgressData = new AcquisitionData(data.bestiaryProgressData);
            this.m_skills = data.skills;
            this.m_soulSkillData = data.soulSkillData;
            m_combatArtsData = data.combatArtsData;
        }

        public void SetPrimarySkillData(PrimarySkillsData data)
        {
            m_skills = data;
        }
#if UNITY_EDITOR
        [NonSerialized, ShowInInspector, BoxGroup("Debug")]
        private BestiaryList m_bestiaryList;

        [Button, BoxGroup("Debug")]
        private void Initialize()
        {
            if (m_bestiaryList)
            {
                InitializeAcquisitionData(ref m_bestiaryProgressData, m_bestiaryList.GetIDs());
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