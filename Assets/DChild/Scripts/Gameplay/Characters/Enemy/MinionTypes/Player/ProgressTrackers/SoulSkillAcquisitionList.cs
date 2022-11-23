using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class SoulSkillAcquiredEventArgs : IEventActionArgs
    {
        public void Initialize(SoulSkill skill, bool isAcquired)
        {
            this.skill = skill;
            this.isAcquired = isAcquired;
        }

        public SoulSkill skill { get; private set; }
        public bool isAcquired { get; private set; }
    }

    public class SoulSkillAcquisitionList : MonoBehaviour
    {
        [SerializeField]
        private SoulSkillList m_list;

        [ShowInInspector, HideInEditorMode, OnValueChanged("SendEvents")]
        private Dictionary<int, bool> m_soulSkills;

        public SoulSkillList list => m_list;

        public event EventAction<SoulSkillAcquiredEventArgs> SkillAcquisistionChanged;

        public bool IsAcquired(int ID)
        {
            if (m_soulSkills.ContainsKey(ID))
            {
                return m_soulSkills[ID];
            }
            else
            {
                return false;
            }
        }

        public void SetAcquisition(int ID, bool value)
        {
            if (m_soulSkills.ContainsKey(ID))
            {
                m_soulSkills[ID] = value;
                SendSkillAcquisitionEventArgs(m_list.GetInfo(ID), value);
            }
        }

        public AcquisitionData SaveData()
        {
            List<AcquisitionData.SerializeData> serializedDatas = new List<AcquisitionData.SerializeData>();
            foreach (var key in m_soulSkills.Keys)
            {
                serializedDatas.Add(new AcquisitionData.SerializeData(key, m_soulSkills[key]));
            }
            return new AcquisitionData(serializedDatas.ToArray());
        }

        public void LoadData(AcquisitionData saveData)
        {
            if (m_soulSkills == null)
            {
                Initialize();
            }
            var size = saveData.count;
            for (int i = 0; i < size; i++)
            {
                var data = saveData.GetData(i);
                if (m_soulSkills.ContainsKey(data.ID))
                {
                    m_soulSkills[data.ID] = data.hasData;
                }
            }

            SendSkillAcquisitionEventArgs(null, false);
        }

        private void SendSkillAcquisitionEventArgs(SoulSkill skill, bool isAcquired)
        {
            using (Cache<SoulSkillAcquiredEventArgs> cacheEventArgs = Cache<SoulSkillAcquiredEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(skill, isAcquired);
                SkillAcquisistionChanged?.Invoke(this, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        private void Awake()
        {
            if (m_soulSkills == null)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            m_soulSkills = new Dictionary<int, bool>();
            var ids = m_list.GetIDs();
            for (int i = 0; i < ids.Length; i++)
            {
                m_soulSkills.Add(ids[i], false);
            }
        }

#if UNITY_EDITOR
        private void SendEvents()
        {
            SendSkillAcquisitionEventArgs(null, false);
        }
#endif
    }
}