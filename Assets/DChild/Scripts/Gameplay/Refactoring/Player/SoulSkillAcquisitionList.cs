using DChild.Gameplay.Characters.Players.SoulSkills;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public struct SoulSkillAcquiredEventArgs : IEventActionArgs
    {
        public SoulSkillAcquiredEventArgs(SoulSkill skill, bool isAcquired) : this()
        {
            this.skill = skill;
            this.isAcquired = isAcquired;
        }

        public SoulSkill skill { get; }
        public bool isAcquired { get; }
    }

    public class SoulSkillAcquisitionList : MonoBehaviour
    {
        [SerializeField]
        private SoulSkillList m_list;

        [ShowInInspector, HideInEditorMode, OnValueChanged("SendEvent")]
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
                SkillAcquisistionChanged?.Invoke(this, new SoulSkillAcquiredEventArgs(m_list.GetInfo(ID), value));
            }
        }

        private void Awake()
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
            SkillAcquisistionChanged?.Invoke(this, new SoulSkillAcquiredEventArgs(null, false));
        }
#endif
    }
}