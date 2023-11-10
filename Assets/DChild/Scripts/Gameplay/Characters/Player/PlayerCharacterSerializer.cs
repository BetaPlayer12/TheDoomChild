using DChild.Gameplay.Inventories;
using DChild.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.SoulSkills;
using DChild.Menu.Codex;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerCharacterSerializer
    {
        [SerializeField, TabGroup("Inventory")]
        private PlayerInventory m_inventory;
        [SerializeField, TabGroup("Codex")]
        private CodexProgressSerializer m_codex;
        [SerializeField, TabGroup("Skill")]
        private PlayerSkills m_playerSkills;
        [SerializeField, TabGroup("Equipped Soul Skill")]
        private PlayerSoulSkillHandle m_soulSkillHandle;
        [SerializeField]
        private CombatArts m_combatArts;
        [SerializeField]
        private PlayerWeapon m_playerWeapon;

        public PlayerCharacterData SaveData()
        {
            return new PlayerCharacterData(m_inventory.Save(),
                                        m_codex.SaveData(),
                                        m_playerSkills.SaveData(),
                                        m_soulSkillHandle.SaveData(),
                                        m_combatArts.SaveData(),
                                        m_playerWeapon.SaveData());
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_inventory.Load(data.inventoryData);
            m_codex.LoadData(data.codexData);
            m_playerSkills.LoadData(data.skills);
            m_soulSkillHandle.LoadData(data.soulSkillData);
            m_combatArts.LoadData(data.combatArtsData);
            m_playerWeapon.LoadData(data.weaponUpgradeSaveData);
        }
    }
}