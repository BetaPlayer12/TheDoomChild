using DChild.Gameplay.Inventories;
using DChild.Serialization;
using DChild.Gameplay.Characters.Players;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players.SoulSkills;
using Sirenix.OdinInspector;

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
        [SerializeField, TabGroup("Soul Skill")]
        private SoulSkillAcquisitionList m_soulSkillAcquisitionList;
        [SerializeField, TabGroup("Equipped Soul Skill")]
        private SoulSkillHandle m_soulSkillHandle;

        public PlayerCharacterData SaveData()
        {
            return new PlayerCharacterData(m_inventory.Save(),
                                        m_bestiaryProgress.SaveData(),
                                        m_playerSkills.SaveData(),
                                        m_soulSkillAcquisitionList.SaveData(),
                                        m_soulSkillHandle.SaveData());
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_inventory.Load(data.inventoryData);
            m_bestiaryProgress.LoadData(data.bestiaryProgressData);
            m_playerSkills.LoadData(data.skills);
            m_soulSkillAcquisitionList.LoadData(data.soulSkillAcquisitionData);
            m_soulSkillHandle.ClearAllSlots();
            m_soulSkillHandle.AttachSkill(GetSoulSkillFromList(data.equippedSoulSkillData.armorSkill));
            m_soulSkillHandle.AttachSkill(GetSoulSkillFromList(data.equippedSoulSkillData.supportSkill));
            m_soulSkillHandle.AttachSkill(GetSoulSkillFromList(data.equippedSoulSkillData.weaponSkill1), 0);
            m_soulSkillHandle.AttachSkill(GetSoulSkillFromList(data.equippedSoulSkillData.weaponSkill2), 0);
        }

        private SoulSkill GetSoulSkillFromList(int ID)
        {
            return m_soulSkillAcquisitionList.list.GetInfo(ID);
        }
    }
}