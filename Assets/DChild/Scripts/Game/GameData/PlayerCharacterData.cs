using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Characters.Players.SoulSkills;
using System;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Characters.Players;
using DChild.Menu.Codex;
using DChild.Gameplay.Systems;

namespace DChild.Serialization
{
    [System.Serializable]
    public class PlayerCharacterData
    {
        [SerializeField, TabGroup("Inventory"), HideReferenceObjectPicker, HideLabel]
        private TradableInventorySerialization m_inventoryData;
        [SerializeField, TabGroup("Codex"), HideReferenceObjectPicker, HideLabel]
        private CodexSaveData m_codexData;
        [SerializeField, TabGroup("Skills")]
        private PrimarySkillsData m_skills;
        [SerializeField, TabGroup("Soul Skill")]
        private PlayerSoulSkillData m_soulSkillData;
        [SerializeField]
        private CombatArtsSaveData m_combatArtsData;
        [SerializeField]
        private WeaponUpgradeSaveData m_weaponUpgradeSaveData;

        public TradableInventorySerialization inventoryData => m_inventoryData;
        public CodexSaveData codexData { get => m_codexData; }
        public PrimarySkillsData skills { get => m_skills; }
        public PlayerSoulSkillData soulSkillData { get => m_soulSkillData; }
        public CombatArtsSaveData combatArtsData { get => m_combatArtsData; }
        public WeaponUpgradeSaveData weaponUpgradeSaveData { get => m_weaponUpgradeSaveData;  }

        public PlayerCharacterData()
        {
            m_inventoryData = new TradableInventorySerialization();
            m_codexData = new CodexSaveData();
            m_skills = new PrimarySkillsData();
            m_soulSkillData = new PlayerSoulSkillData();
            m_combatArtsData = new CombatArtsSaveData(new Gameplay.Characters.Player.CombatArt.Leveling.CombatArtLevel.SaveData(),0, new int[0]);
        }

        public PlayerCharacterData(TradableInventorySerialization m_inventoryData, CodexSaveData m_codexData, PrimarySkillsData m_skills, PlayerSoulSkillData m_soulSkillData, CombatArtsSaveData combatArtsData, WeaponUpgradeSaveData weaponUpgradeSaveData)
        {
            this.m_inventoryData = m_inventoryData;
            this.m_codexData = m_codexData;
            this.m_skills = m_skills;
            this.m_soulSkillData = m_soulSkillData;
            this.m_weaponUpgradeSaveData = weaponUpgradeSaveData;
            m_combatArtsData = combatArtsData;
        }

        public PlayerCharacterData(PlayerCharacterData data)
        {
            this.m_inventoryData = new TradableInventorySerialization(data.inventoryData);
            this.m_codexData = new CodexSaveData(data.codexData);
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
            //if (m_bestiaryList)
            //{
            //    InitializeAcquisitionData(ref m_bestiaryProgressData, m_bestiaryList.GetIDs());
            //}
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