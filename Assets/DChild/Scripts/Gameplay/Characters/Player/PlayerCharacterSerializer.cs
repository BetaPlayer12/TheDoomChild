using DChild.Gameplay.Inventories;
using DChild.Serialization;
using DChild.Gameplay.Characters.Players;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players.SoulSkills;
using Sirenix.OdinInspector;
using DChild.Gameplay.SoulSkills;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerCharacterSerializer
    {
        [SerializeField, TabGroup("Inventory")]
        private PlayerInventory m_inventory;
        [SerializeField, TabGroup("Bestiary")]
        private BestiaryProgress m_bestiaryProgress;
        [SerializeField, TabGroup("Skill")]
        private PlayerSkills m_playerSkills;
        [SerializeField, TabGroup("Equipped Soul Skill")]
        private PlayerSoulSkillHandle m_soulSkillHandle;

        public PlayerCharacterData SaveData()
        {
            return new PlayerCharacterData(m_inventory.Save(),
                                        m_bestiaryProgress.SaveData(),
                                        m_playerSkills.SaveData(),
                                        m_soulSkillHandle.SaveData());
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_inventory.Load(data.inventoryData);
            m_bestiaryProgress.LoadData(data.bestiaryProgressData);
            m_playerSkills.LoadData(data.skills);
            m_soulSkillHandle.LoadData(data.soulSkillData);
        }
    }
}