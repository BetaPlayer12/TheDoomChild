using DChild.Gameplay.Inventories;
using DChild.Serialization;
using Refactor.DChild.Gameplay.Characters.Players;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerCharacterSerializer
    {
        [SerializeField]
        private PlayerInventory m_inventory;
        [SerializeField]
        private BestiaryProgress m_bestiaryProgress;
        [SerializeField]
        private PlayerSkills m_playerSkills;
        [SerializeField]
        private SoulSkillAcquisitionList m_soulSkillAcquisitionList;
        [SerializeField]
        private SoulSkillHandle m_soulSkillHandle;

        public PlayerCharacterData SaveData()
        {
            var bestiaryProgressData = m_bestiaryProgress.SaveData();
            var primarySkillsData = m_playerSkills.SaveData();
            var soulSkillAcquisitionData = m_soulSkillAcquisitionList.SaveData();
            return new PlayerCharacterData();
        }

        public void LoadData(PlayerCharacterData data)
        {
            m_bestiaryProgress.LoadData(data.bestiaryProgressData);
            m_playerSkills.LoadData(data.skills);
            m_soulSkillAcquisitionList.LoadData(data.soulSkillAcquisitionData);
            m_soulSkillHandle.ClearAllSlots();
        }
    }
}