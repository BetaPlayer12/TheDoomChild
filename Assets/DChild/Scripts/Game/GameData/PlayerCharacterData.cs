using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DChild.Menu.Bestiary;
using DChild.Gameplay.Characters.Players.SoulSkills;

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


#if UNITY_EDITOR
        [SerializeField, BoxGroup("Debug")]
        private BestiaryList m_bestiaryList;
        [SerializeField, BoxGroup("Debug")]
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