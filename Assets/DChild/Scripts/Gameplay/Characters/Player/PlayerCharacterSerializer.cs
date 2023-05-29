﻿using DChild.Gameplay.Inventories;
using DChild.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Leveling;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerCharacterSerializer
    {
        [SerializeField]
        private PlayerLevel m_level;
        [SerializeField, TabGroup("Inventory")]
        private PlayerInventory m_inventory;
        [SerializeField, TabGroup("Bestiary")]
        private BestiaryProgress m_bestiaryProgress;
        [SerializeField, TabGroup("Skill")]
        private PlayerSkills m_playerSkills;
        [SerializeField, TabGroup("Equipped Soul Skill")]
        private PlayerSoulSkillHandle m_soulSkillHandle;
        [SerializeField]
        private CombatArts m_combatArts;

        public PlayerCharacterData SaveData()
        {
            return new PlayerCharacterData(m_level.Save(),
                                        m_inventory.Save(),
                                        m_bestiaryProgress.SaveData(),
                                        m_playerSkills.SaveData(),
                                        m_soulSkillHandle.SaveData(),
                                        m_combatArts.SaveData());
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_level.Load(data.levelData);
            m_inventory.Load(data.inventoryData);
            m_bestiaryProgress.LoadData(data.bestiaryProgressData);
            m_playerSkills.LoadData(data.skills);
            m_soulSkillHandle.LoadData(data.soulSkillData);
            m_combatArts.LoadData(data.combatArtsData);
        }
    }
}