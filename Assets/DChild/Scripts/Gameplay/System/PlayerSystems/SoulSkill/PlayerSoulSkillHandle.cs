using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Serialization;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{

    public class PlayerSoulSkillHandle : SerializedMonoBehaviour, ISerializable<PlayerSoulSkillData>
    {
        [SerializeField]
        private PlayerSoulSkillsConfiguration m_playerSoulSkillsConfiguration;
        [SerializeField]
        private IPlayer m_player;
        [SerializeField, MinValue(1)]
        private int m_maxActivatedSoulSkill = 9;
        [SerializeField, MinValue(1)]
        private int m_maxSoulCapacity = 1;

        [ShowInInspector, HideInEditorMode]
        private int m_currentSoulCapacity;
        private HashSet<int> m_acquiredSkills;
        private HashSet<int> m_activatedSkillsID;
        private HashSet<SoulSkill> m_activatedSkills;

        public int maxSoulCapacity => m_maxSoulCapacity;
        public int currentSoulCapacity => m_currentSoulCapacity;
        public IReadOnlyCollection<int> acquiredSkills => m_acquiredSkills;
        public IReadOnlyCollection<int> activatedSkills => m_activatedSkillsID;

        public event EventAction<EventActionArgs> SaveDataLoaded;
        public event EventAction<EventActionArgs> AvailableSoulSkillChanged;
        public event EventAction<EventActionArgs> MaxCapacityChanged;


        public PlayerSoulSkillData SaveData()
        {
            return new PlayerSoulSkillData(m_maxSoulCapacity, m_acquiredSkills.ToArray(), m_activatedSkillsID.ToArray());
        }

        public void LoadData(PlayerSoulSkillData data)
        {
            m_acquiredSkills.Clear();
            RemoveAllActiveSoulSkills();

            if (data != null)
            {
                for (int i = 0; i < data.acquiredSoulSkills.Length; i++)
                {
                    m_acquiredSkills.Add(data.acquiredSoulSkills[i]);
                }

                for (int i = 0; i < data.activatedSoulSkills.Length; i++)
                {
                    m_activatedSkillsID.Add(data.activatedSoulSkills[i]);
                }

                m_currentSoulCapacity = Mathf.Clamp(data.currentSoulCapacity, 0, m_maxSoulCapacity);
            }

            SaveDataLoaded?.Invoke(this, EventActionArgs.Empty);
        }

        public void AddAsAcquired(int soulSkillID)
        {
            m_acquiredSkills.Add(soulSkillID);
            AvailableSoulSkillChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public void RemoveAsAcquired(int soulSkillID)
        {
            m_acquiredSkills.Remove(soulSkillID);
            AvailableSoulSkillChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public bool CanBeActivated(SoulSkill soulSkill)
        {
            if (m_activatedSkillsID == null)
            {
                //Temporary For Testing
                return true;
            }
            return m_maxActivatedSoulSkill >= m_activatedSkillsID.Count + 1 && (m_currentSoulCapacity - soulSkill.capacity) >= 0;
        }

        public bool HasAcquired(SoulSkill soulSkill)
        {
            if (m_acquiredSkills == null)
            {
                //Temporary For Testing
                return true;
            }

            return m_acquiredSkills.Contains(soulSkill.id);
        }

        public void AddAsActivated(SoulSkill soulSkill)
        {
            if (m_acquiredSkills.Contains(soulSkill.id))
            {
                m_activatedSkillsID.Add(soulSkill.id);
                m_activatedSkills.Add(soulSkill);
                soulSkill.AttachTo(m_player);
                m_currentSoulCapacity -= soulSkill.capacity;
            }
        }

        public void RemoveAsActivated(SoulSkill soulSkill)
        {
            if (m_activatedSkillsID.Contains(soulSkill.id))
            {
                m_activatedSkillsID.Remove(soulSkill.id);
                m_activatedSkills.Remove(soulSkill);
                soulSkill.DetachFrom(m_player);
                m_currentSoulCapacity += soulSkill.capacity;
            }
        }

        public void AddMaxCapacity(int additionCapacity)
        {
            m_maxSoulCapacity += additionCapacity;
            m_currentSoulCapacity += additionCapacity;
            MaxCapacityChanged?.Invoke(this, EventActionArgs.Empty);
        }

        public void SetMaxCapacity(int maxCapacity)
        {
            m_maxSoulCapacity = maxCapacity;
            MaxCapacityChanged?.Invoke(this, EventActionArgs.Empty);
            m_currentSoulCapacity = Mathf.Min(m_currentSoulCapacity, m_maxSoulCapacity);
        }

        public void Initialize()
        {
            m_maxActivatedSoulSkill = m_playerSoulSkillsConfiguration.maxActivatedSoulSkill;
            m_maxSoulCapacity = m_playerSoulSkillsConfiguration.maxSoulCapacity;

            m_acquiredSkills = new HashSet<int>();
            m_activatedSkillsID = new HashSet<int>();
            m_activatedSkills = new HashSet<SoulSkill>();

            m_player.inventory.SoulSkillItemAcquired += OnSoulSkillItemAcquired;

            m_currentSoulCapacity = m_maxSoulCapacity;
        }

        private void OnSoulSkillItemAcquired(object sender, SoulSkillAcquiredEventArgs eventArgs)
        {
            AddAsAcquired(eventArgs.ID);
        }

        private void RemoveAllActiveSoulSkills()
        {
            for (int i = 0; i < m_activatedSkills.Count; i++)
            {
                RemoveAsActivated(m_activatedSkills.ElementAt(i));
            }
            m_activatedSkills.Clear();
            m_activatedSkillsID.Clear();
        }
    }
}
